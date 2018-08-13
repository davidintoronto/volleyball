using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VballManager
{
    public class Constants
    {
        public const String DATA = "Data";
        public const String GAME_DATE = "GameDate";
        public const String CURRENT_PLAYER_ID = "CurrentUserId";
        public const String DATAFILE = @"\App_Data\Reservation.data.";
        public const String IP_FILE = @"\App_Data\HomeIP.data.";
        public const String PAGE_MODE = "PageMode";
        public const int IMAGE_BUTTON_SIZE = 128;
        public const String MESSAGE = "Message";
        public const String PASSCODE = "Passcode";
        public const String LINKBUTTON_FONTSIZE = "1em";
        public const String CONTROL = "Control";
        public const String ACTION_TYPE = "ActionType";
        public const String ACTION_MEMBER_ATTEND = "MemberAttend";
        public const String ACTION_DETAIL = "Detail";
        public const String ACTION_ADD_WAITING_LIST = "AddWaitingList";
        public const String ACTION_DROPIN_NEW = "Drop-inNew";
        public const String ACTION_DROPIN_REMOVE = "Drop-inRemove";
        public const String ACTION_DROPIN_ADD = "Drop-inAdd";
        public const String ACTION_POWER_RESERVE = "PowerReserve";
        public const String ACTION_MOVE_RESERVATION = "MoveReservation";
        public const String SUPER_ADMIN = "SuperAdmin";
        public const String POOL = "Pool";
        public const String POOL_ID = "abcd";
        public const String PAYMENT_ID = "PaymentId";
        public const String FEE_ID = "FeeId";
        public const String UNLOCK = "Unlock";
        public const String APP_LOCKED = "AppLocked";
        public const String PLAYER_ID = "PlayerId";
        public const String USER_ID = "UserId";
        public const int TWELVE = 12;
        public const String ACTION_MEMEBER_ADD = "MemberAdd";
        public const String ADD_PLAYER_POOL = "AddPlayerToPool";
        public const String PRIMARY_USER = "PrimaryUser";
        public const String CURRENT_USER = "CurrentUser";
        public const String RESERVED = "reserved a spot", CANCELLED = "cancelled the spot", WAITING = "put you on the waiting list", WAITING_TO_RESERVED = "Congratus! You got a spot", MOVED = "Your spot is moved to pool";
        public const String REQUEST_REGISTER_LINK_PAGE = "RequestRegisterLink.aspx";
        public const String POOL_LINK_LIST_PAGE = "PoolLinkList.aspx";
        public const String AUTHORIZE_USER_PAGE = "Authorize.aspx";
        public const String REGISTER_DEVICE_PAGE = "RegisterDevice.aspx";
        public const String HOME_PC_IP = "HomePcIp";
    }
}