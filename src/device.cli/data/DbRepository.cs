using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using AutoMapper;
using forte.devices.data.enums;
using forte.devices.entities;
using forte.devices.Migrations;
using forte.devices.services;
using forte.models;
using forte.services;

namespace forte.devices.data
{
	public class DbRepository
	{
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public DbRepository(IRuntimeConfig cf, ILogger logger)
        {
            _logger = logger;

			var dataPath = cf.DataPath;

			if(!Directory.Exists(dataPath))
			{
				Directory.CreateDirectory(dataPath);
			}

            var dbFilePath = Path.Combine(dataPath, "device-db.sdf");

            _connectionString = $"Data Source={dbFilePath}";
        }

		#region sessions

		public SessionState GetSession(Guid sessionId)
		{
			using(var dbContext = new DeviceDbContext(_connectionString))
			{
				return dbContext.SessionStates.FirstOrDefault(s => s.Status != WorkflowState.Processed && s.SessioId == sessionId);
			}
		}

		public List<SessionState> GetSessions()
		{
			using(var dbContext = new DeviceDbContext(_connectionString))
			{
				return dbContext.SessionStates.Where(s => s.Status != WorkflowState.Processed).OrderBy(s => s.StartTime).ToList();
			}
		}

		public void UpdateSession(SessionState session)
		{
			using(var dbContext = new DeviceDbContext(_connectionString))
			{
				dbContext.SessionStates.Attach(session);
				dbContext.Entry(session).State = EntityState.Modified;
				dbContext.SaveChanges();
			}
		}

		public void CreateSession(SessionState session)
		{
			using(var dbContext = new DeviceDbContext(_connectionString))
			{
				dbContext.SessionStates.Add(session);
				dbContext.SaveChanges();
			}
		}

		#endregion

		#region settings

		public List<DeviceSetting> GetSettings(bool automigrate = true)
		{
			using(var dbContext = new DeviceDbContext(_connectionString))
			{
				try
				{
					return dbContext.Settings.ToList();
				}
				catch(InvalidOperationException ex)
				{
					if(!automigrate) throw;
					_logger.Error(ex, $"Database might not be up to date, try migrating.");

					var configuration = new Configuration();
					configuration.TargetDatabase = new DbConnectionInfo("local-db");
					var migrator = new DbMigrator(configuration);
					migrator.Update();
					return GetSettings(false);
				}
			}
		}

		public void SaveSetting<T>(string setting, T value)
		{
			DeviceSetting existing;

			using(var dbContext = new DeviceDbContext(_connectionString))
			{
				// ... want it deached, hence calling separately
				existing = dbContext.Settings.FirstOrDefault(s => s.Name == setting);
			}

			using(var dbContext = new DeviceDbContext(_connectionString))
			{
				var newSetting = Mapper.Map<DeviceSetting>(new DataValue(value));
				newSetting.Name = setting;
				newSetting.LastModified = DateTime.UtcNow;

				if(existing != null)
				{
					newSetting.Id = existing.Id;
					newSetting.Version = existing.Version;
					newSetting.Created = existing.Created;
					dbContext.Settings.Attach(newSetting);
					dbContext.Entry(newSetting).State = EntityState.Modified;
				}
				else
				{
					newSetting.Id = Guid.NewGuid();
					newSetting.Created = DateTime.UtcNow;
					dbContext.Settings.Add(newSetting);
				}

				dbContext.SaveChanges();
			}
		}

		public List<DeviceSetting> SaveSettings(List<DeviceSetting> settings)
		{
			using(var dbContext = new DeviceDbContext(_connectionString))
			{
				var savedSettings = GetSettings();

				foreach(var setting in savedSettings)
				{
					if(settings.All(s => s.Name != setting.Name))
						dbContext.Settings.Remove(setting);
				}

				foreach(var setting in settings)
				{
					var existing = savedSettings.FirstOrDefault(s => s.Name == setting.Name);
					if(existing != null)
					{
						setting.Id = existing.Id;
						setting.Version = existing.Version;
						dbContext.Settings.Attach(setting);
						dbContext.Entry(setting).State = EntityState.Modified;
					}
					else
					{
						setting.Id = Guid.NewGuid();
						setting.Created = DateTime.UtcNow;
						dbContext.Settings.Add(setting);
					}
				}

				dbContext.SaveChanges();

				return dbContext.Settings.ToList();
			}
		}

		#endregion
	}
}