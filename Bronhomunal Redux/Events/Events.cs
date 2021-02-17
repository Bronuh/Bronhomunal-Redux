using Bronuh.Types;
using System;
using System.Threading.Tasks;

namespace Bronuh.Events
{
	/// TODO: Добавить систему событий, для улучшения читаемости кода

	public delegate Task AsyncEventHandler<in TSender, in TArgs>(TSender sender, TArgs e) where TArgs : AsyncEventArgs;
	public class AsyncEventArgs : EventArgs
	{
		public AsyncEventArgs() { }

		public bool Handled { get; set; }
	}




	public delegate void MemberGotAchievement(Member sender, MemberGotAchievementEventArgs eventArgs);
	public class MemberGotAchievementEventArgs : AsyncEventArgs
	{
		public MemberGotAchievementEventArgs() { }
		public MemberGotAchievementEventArgs(Achievement achievement)
		{
			Achievement = achievement;
		}
		public Achievement Achievement;
	}

	public delegate void MemberGotXp(Member sender, MemberGotXpEventArgs eventArgs);
	public class MemberGotXpEventArgs : AsyncEventArgs
	{
		public MemberGotXpEventArgs() { }
		public MemberGotXpEventArgs(int xp)
		{
			Xp = xp;
		}
		public int Xp;
	}

	public delegate void MemberRankedUp(Member sender, MemberRankedUpEventArgs eventArgs);
	public class MemberRankedUpEventArgs : AsyncEventArgs
	{

	}

	public delegate void MemberJoinedVoice(Member sender, MemberJoinedVoiceEventArgs eventArgs);
	public class MemberJoinedVoiceEventArgs : AsyncEventArgs
	{

	}

	public delegate void MemberLeavedVoice(Member sender, MemberLeavedVoiceEventArgs eventArgs);
	public class MemberLeavedVoiceEventArgs : AsyncEventArgs
	{

	}

	public delegate void MemberUpdated(Member sender, MemberUpdatedEventArgs eventArgs);
	public class MemberUpdatedEventArgs : AsyncEventArgs
	{

	}

	public delegate void MemberSentMessage(Member sender, MemberSentMessageEventArgs eventArgs);
	public class MemberSentMessageEventArgs : AsyncEventArgs
	{
		public MemberSentMessageEventArgs(ChatMessage message)
		{
			Message = message;
		}
		public ChatMessage Message;
	}

	public delegate void MemberExecutedCommand(Member sender, MemberExecutedCommandEventArgs eventArgs);
	public class MemberExecutedCommandEventArgs : AsyncEventArgs
	{
		public MemberExecutedCommandEventArgs(ChatMessage message, Command command)
		{
			Message = message;
			Command = command;
		}
		public ChatMessage Message;
		public Command Command;
	}

	public delegate void MemberUsedMention(Member sender, MemberUsedMentionEventArgs eventArgs);
	public class MemberUsedMentionEventArgs : AsyncEventArgs
	{
		public Mention Mention;
		public Member Author;
		public Member Target;
	}

	public delegate void MemberVoiceLeavedMention(Member sender, MemberVoiceLeavedEventArgs eventArgs);
	public class MemberVoiceLeavedEventArgs : AsyncEventArgs
	{

	}
}
