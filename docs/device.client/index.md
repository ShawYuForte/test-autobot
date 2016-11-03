# Streaming Device Daemon

The device Daemon runs continuously on the streaming device machine, and has the following responsibilities:

* Responding to and running server commands
* Ensuring continuous operation
* Logging

## Roadmap

The following may or may not be in the VSTS backlog:

* Check periodically even between commands that the current command execution is ok. E.g. if streaming, check periodically even between commands to make sure nothing broke