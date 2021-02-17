using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bronuh.Types;

namespace Bronuh.Events
{
	public class MessageCreatedEventArgs : AsyncEventArgs { }
	public class CommandCalledEventArgs : AsyncEventArgs { }
	public class CommandExecutedEventArgs : AsyncEventArgs {

		public ChatMessage Message;
		public CommandExecutedEventArgs() { }
		public CommandExecutedEventArgs(ChatMessage message) { Message = message; }
		
	}
	public class CommandCancelledEventArgs : AsyncEventArgs { }
}
