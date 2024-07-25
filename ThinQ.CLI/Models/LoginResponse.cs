namespace ThinQ.CLI.Models;

public record LoginResponse(
    Account account
)
{
    public string UserId => account.userID;
    public string UserIdType => account.userIDType;
    public string Country => account.country;
    public string LoginSessionId => account.loginSessionId;
}

public record Account(
    string loginSessionId,
    string userID,
    string userNo,
    string userIDType,
    string firstName,
    string lastName,
    string dateOfBirth,
    string country,
    string countryName,
    string blacklist,
    string age,
    string isSubscribe,
    string isReceiveSms,
    string changePw,
    string toEmailId,
    string periodPW,
    string lgAccount,
    string isService,
    object[] termsList,
    UserIDList[] userIDList,
    ServiceList[] serviceList,
    string displayUserID,
    NotiList notiList,
    string authUser,
    string dummyIdFlag,
    string pwChgDatetime,
    string lastLognDate,
    string crtDate
);

public record UserIDList(
    LgeIDList[] lgeIDList,
    ThirdPartyIDList[] thirdPartyIDList
);

public record LgeIDList(
    string lgeIDType,
    string userID
);

public record ThirdPartyIDList(
    string thirdParty,
    string thirdPartyID,
    string thirdPartyEmail
);

public record ServiceList(
    string svcCode,
    string svcName,
    string isService,
    string joinDate
);

public record NotiList(
    string totCount,
    object[] list
);
