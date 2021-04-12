namespace Necromancy.Server.Chat
{
    public enum ChatErrorType
    {
        Success = 0,
        GenericUnknownStatement = 1,
        YouAreUnableToWhisperToYourself = 2,
        YouAreNotAbleToSpeakSinceYouAreNotInParty = -802,
        UnableToFindSoul = -1400,
        YouDoNotHaveAnAllChatItem = -1401,
        ActionFailedSinceItIsOnCoolDown1402 = -1402,
        YouMayNotRepeatPhrasesOverAndOverInChat = -1403,
        YouMayNotShoutAtSoulRank1 = -1404,
        ActionFailedSinceItIsOnCoolDown1405 = -1405,
        TargetRefusedToAcceptYourWhisper = -2201,
        UnableToWhisperAsYouAreOnTheRecipientsBlockList = -2202,
        YouMayNotChatInAllChatDuringTrades = -3000,
        YouMayNotChatInAllChatWhileYouHaveShopOpen = -3001
    }
}
