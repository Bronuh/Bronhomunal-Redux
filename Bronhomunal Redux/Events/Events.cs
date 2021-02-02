using Bronuh.Types;
using System;

namespace Bronuh.Events
{
	/// TODO: Добавить систему событий, для улучшения читаемости кода

	public delegate void MemberGotAchievement(Member sender, MemberGotAchievementEventArgs eventArgs);
	public class MemberGotAchievementEventArgs : EventArgs
	{
		public MemberGotAchievementEventArgs() { }
		public MemberGotAchievementEventArgs(Achievement achievement)
		{
			Achievement = achievement;
		}
		public Achievement Achievement;
	}

	public delegate void MemberGotXp(Member sender, MemberGotXpEventArgs eventArgs);
	public class MemberGotXpEventArgs : EventArgs
	{
		public MemberGotXpEventArgs() { }
		public MemberGotXpEventArgs(int xp)
		{
			Xp = xp;
		}
		public int Xp;
	}

	public delegate void MemberRankedUp(Member sender, MemberRankedUpEventArgs eventArgs);
	public class MemberRankedUpEventArgs : EventArgs
	{

	}

	public delegate void MemberJoinedVoice(Member sender, MemberJoinedVoiceEventArgs eventArgs);
	public class MemberJoinedVoiceEventArgs : EventArgs
	{

	}

	public delegate void MemberLeavedVoice(Member sender, MemberLeavedVoiceEventArgs eventArgs);
	public class MemberLeavedVoiceEventArgs : EventArgs
	{

	}

	public delegate void MemberUpdated(Member sender, MemberUpdatedEventArgs eventArgs);
	public class MemberUpdatedEventArgs : EventArgs
	{

	}

	public delegate void MemberSentMessage(Member sender, MemberSentMessageEventArgs eventArgs);
	public class MemberSentMessageEventArgs : EventArgs
	{
		public MemberSentMessageEventArgs(ChatMessage message)
		{
			Message = message;
		}
		public ChatMessage Message;
	}

	public delegate void MemberExecutedCommand(Member sender, MemberExecutedCommandEventArgs eventArgs);
	public class MemberExecutedCommandEventArgs : EventArgs
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
	public class MemberUsedMentionEventArgs : EventArgs
	{
		public Mention Mention;
		public Member Author;
		public Member Target;
	}
}
