// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Serilog.Sinks.PeriodicBatching;
using LogEvent = device.logging.sinks.signalr.data.LogEvent;

namespace device.logging.sinks.signalr
{
    /// <summary>
    /// Writes log events as messages to a SignalR hub.
    /// </summary>
    public class SignalRSink : PeriodicBatchingSink
    {
        readonly IFormatProvider _formatProvider;
        readonly IHubContext _context;
        /// <summary>
        /// A reasonable default for the number of events posted in
        /// each batch.
        /// </summary>
        public const int DefaultBatchPostingLimit = 1;

        /// <summary>
        /// A reasonable default time to wait between checking for event batches.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Construct a sink posting to the specified database.
        /// </summary>
        /// <param name="context">The hub context.</param>
        /// <param name="batchPostingLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        public SignalRSink(IHubContext context, int batchPostingLimit, TimeSpan period, IFormatProvider formatProvider)
            : base(batchPostingLimit, period)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _formatProvider = formatProvider;
            _context = context;
        }

        /// <summary>
        /// Emit a batch of log events, running asynchronously.
        /// </summary>
        /// <param name="events">The events to emit.</param>
        /// <remarks>Override either <see cref="PeriodicBatchingSink.EmitBatch"/> or <see cref="PeriodicBatchingSink.EmitBatchAsync"/>,
        /// not both.</remarks>
        protected override void EmitBatch(IEnumerable<Serilog.Events.LogEvent> events)
        {
            // This sink doesn't use batching to send events, instead only using
            // PeriodicBatchingSink to manage the worker thread; requires some consideration.

            foreach (var logEvent in events)
            {
                var logEventJson =
                    JsonConvert.SerializeObject(new LogEvent(logEvent, logEvent.RenderMessage(_formatProvider)));
                _context.Clients.All.sendLogEvent(logEventJson);
            }
        }
    }
}
