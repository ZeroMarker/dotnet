using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Xml.Linq;

namespace ReadCardDK
{
    public class ReadCard
    {
        [DllImport("dcrf32.dll")]
        public static extern int dc_init(int port, int type);
        [DllImport("dcrf32.dll")]
        public static extern int dc_beep(int handel, int times);
        [DllImport("dcrf32.dll")]
        public static extern int dc_SamAReadCardInfo(int handel, int type, out int msgLen, ref byte pMsg, out int photoLen, ref byte pPhoto, out int fingerLen, ref byte pFinger, out int extraLen, ref byte pExtra);
        [DllImport("dcrf32.dll")]
        //public static extern int dc_ParsePhotoInfo(int handel, int type, int photoLen, ref byte pPhoto, out int base64Len, ref byte pBase64);
        public static extern int dc_ParsePhotoInfo(int handel, int type, int photoLen, ref byte pPhoto, out int base64Len, out byte pBase64);
        [DllImport("dcrf32.dll")]
        public static extern int dc_exit(int port);
        [DllImport("SSCardDriver.dll")]
        public static extern int iReadCardBas(int type, StringBuilder pMsg);

        [DllImport("dcrf32.dll")]
        public static extern short dc_ParseTextInfo(int handle, int charset, int info_len, [Out] byte[] info, [Out] byte[] name, [Out] byte[] sex, [Out] byte[] nation, [Out] byte[] birth_day, [Out] byte[] address, [Out] byte[] id_number, [Out] byte[] department, [Out] byte[] expire_start_day, [Out] byte[] expire_end_day, [Out] byte[] reserved);
        [DllImport("dcrf32.dll")]
        public static extern short dc_ParseTextInfoForForeigner(int handle, int charset, int info_len, [Out] byte[] info, [Out] byte[] english_name, [Out] byte[] sex, [Out] byte[] id_number, [Out] byte[] citizenship, [Out] byte[] chinese_name, [Out] byte[] expire_start_day, [Out] byte[] expire_end_day, [Out] byte[] birth_day, [Out] byte[] version_number, [Out] byte[] department_code, [Out] byte[] type_sign, [Out] byte[] reserved);

        [DllImport("dcrf32.dll")]
        public static extern short dc_ParseTextInfoForNewForeigner(int handle, int charset, int info_len, [Out] byte[] info, [Out] byte[] chinese_name, [Out] byte[] sex, [Out] byte[] renew_count, [Out] byte[] birth_day, [Out] byte[] english_name, [Out] byte[] id_number, [Out] byte[] reserved, [Out] byte[] expire_start_day, [Out] byte[] expire_end_day, [Out] byte[] english_name_ex, [Out] byte[] citizenship, [Out] byte[] type_sign, [Out] byte[] prev_related_info, [Out] byte[] old_id_number);

        [DllImport("dcrf32.dll")]
        public static extern short dc_ParsePhotoInfo(int handle, int type, int info_len, [Out] byte[] info, ref int photo_len, [Out] byte[] photo);
        [DllImport("dcrf32.dll")]
        public static extern short dc_ParseOtherInfo(int icdev, int flag, [In] byte[] in_info, [Out] byte[] out_info);

        [DllImport("dcrf32.dll")]
        public static extern short dc_GetIdCardType(int icdev, int info_len, [In] byte[] in_info);

        [DllImport("dcrf32.dll")]
        public static extern short dc_GetSocialSecurityCardBaseInfo(int icdev, int type, [Out] byte[] card_code, [Out] byte[] card_type, [Out] byte[] version, [Out] byte[] init_org_number, [Out] byte[] card_issue_date, [Out] byte[] card_expire_day, [Out] byte[] card_number, [Out] byte[] social_security_number, [Out] byte[] name, [Out] byte[] name_ex, [Out] byte[] sex, [Out] byte[] nation, [Out] byte[] birth_place, [Out] byte[] birth_day);

        public ReadCard()
        {

        }

        public string ReadIDCard(Boolean DetailFlag)
        {
            string strResult = "";
            int Rtn = -1;
            int iHandle = 0;
            for (int iPort = 100; iPort < 200; iPort++)
            {
                iHandle = dc_init(iPort, 0);
                if (iHandle > 0) break;
            }
            if (iHandle > 0)
            {
                dc_beep(iHandle, 20);
                String Name = "";
                String Sex = "";
                String NationDescLookUpRowID = "";
                String Birth = "";
                String Address = "";
                String CredNo = "";
                String Department = "";
                String Expirestartday = "";
                String Expireendday = "";
                String Country = "";
                String EnglishName = "";
                String ChineseName = "";
                String Cardtype = "";
                String PhotoBase64 = "";

                byte[] pMsg = new byte[256];
                byte[] pPhoto = new byte[1024];
                byte[] pFinger = new byte[1024];
                byte[] pExtra = new byte[70];
                byte[] pBase64 = new byte[65536];


                int MsgLen = 0, PhotoLen = 0, FingerLen = 0, ExtraLen = 0, Base64Len = 65536;
                int iRet = dc_SamAReadCardInfo(iHandle, 1, out MsgLen, ref pMsg[0], out PhotoLen, ref pPhoto[0], out FingerLen, ref pFinger[0], out ExtraLen, ref pExtra[0]);
                if (iRet != 0)
                {
                    strResult = "-1^读取身份证信息失败,请确认身份证是否放置,身份证是否有效";

                }
                int type = dc_GetIdCardType(iHandle, MsgLen, pMsg);

                if (type == 0)
                {
                    byte[] name = new byte[64];
                    byte[] sex = new byte[8];
                    byte[] nation = new byte[12];
                    byte[] birth_day = new byte[36];
                    byte[] address = new byte[144];
                    byte[] id_number = new byte[76];
                    byte[] department = new byte[64];
                    byte[] expire_start_day = new byte[36];
                    byte[] expire_end_day = new byte[36];
                    byte[] reserved = new byte[76];
                    byte[] info_buffer = new byte[64];

                    Rtn = dc_ParseTextInfo(iHandle, 0, MsgLen, pMsg, name, sex, nation, birth_day, address, id_number, department, expire_start_day, expire_end_day, reserved);
                    if (Rtn != 0)
                    {
                        strResult = "-1^读取身份证信息失败";
                        dc_exit(iHandle);
                    }
                    Name = System.Text.Encoding.Default.GetString(name);
                    Name = Name.Replace("\0", "");
                    Sex = System.Text.Encoding.Default.GetString(sex);
                    Sex = Sex.Replace("\0", "");
                    NationDescLookUpRowID = System.Text.Encoding.Default.GetString(nation);
                    NationDescLookUpRowID = NationDescLookUpRowID.Replace("\0", "");
                    NationDescLookUpRowID = Convert.ToInt32(NationDescLookUpRowID).ToString();
                    Birth = System.Text.Encoding.Default.GetString(birth_day);
                    Birth = Birth.Replace("\0", "");
                    Address = System.Text.Encoding.Default.GetString(address);
                    Address = Address.Replace("\0", "");
                    CredNo = System.Text.Encoding.Default.GetString(id_number);
                    CredNo = CredNo.Replace("\0", "");
                    Department = System.Text.Encoding.Default.GetString(department);
                    Department = Department.Replace("\0", "");
                    Expirestartday = System.Text.Encoding.Default.GetString(expire_start_day);
                    Expirestartday = Expirestartday.Replace("\0", "");
                    Expireendday = System.Text.Encoding.Default.GetString(expire_end_day);
                    Expireendday = Expireendday.Replace("\0", "");
                    Cardtype = "";
                }
                else if (type == 1)
                {
                    ///老外国人居住证
                    byte[] english_name = new byte[244];
                    byte[] sex = new byte[8];
                    byte[] id_number = new byte[64];
                    byte[] citizenship = new byte[16];
                    byte[] chinese_name = new byte[64];
                    byte[] expire_start_day = new byte[36];
                    byte[] expire_end_day = new byte[36];
                    byte[] birth_day = new byte[36];
                    byte[] version_number = new byte[12];
                    byte[] department_code = new byte[20];
                    byte[] type_sign = new byte[8];
                    byte[] reserved = new byte[16];
                    byte[] info_buffer = new byte[64];

                    Rtn = dc_ParseTextInfoForForeigner(iHandle, 0, MsgLen, pMsg, english_name, sex, id_number, citizenship, chinese_name, expire_start_day, expire_end_day, birth_day, version_number, department_code, type_sign, reserved);
                    if (Rtn != 0)
                    {
                        strResult = "-1^读取外国人信息失败";
                        dc_exit(iHandle);
                    }
                    Name = System.Text.Encoding.Default.GetString(english_name);
                    Name = Name.Replace("\0", "");
                    Sex = System.Text.Encoding.Default.GetString(sex);
                    Sex = Sex.Replace("\0", "");
                    ///国家
                    dc_ParseOtherInfo(iHandle, 2, citizenship, info_buffer);
                    Country = System.Text.Encoding.Default.GetString(info_buffer);
                    Country = Country.Replace("\0", "");

                    Birth = System.Text.Encoding.Default.GetString(birth_day);
                    Birth = Birth.Replace("\0", "");
                    Address = "";
                    CredNo = System.Text.Encoding.Default.GetString(id_number);
                    CredNo = CredNo.Replace("\0", "");
                    Department = System.Text.Encoding.Default.GetString(department_code);
                    Department = Department.Replace("\0", "");
                    Expirestartday = System.Text.Encoding.Default.GetString(expire_start_day);
                    Expirestartday = Expirestartday.Replace("\0", "");
                    Expireendday = System.Text.Encoding.Default.GetString(expire_end_day);
                    Expireendday = Expireendday.Replace("\0", "");
                    ChineseName = System.Text.Encoding.Default.GetString(chinese_name);
                    ChineseName = ChineseName.Replace("\0", "");
                    EnglishName = System.Text.Encoding.Default.GetString(english_name);
                    EnglishName = EnglishName.Replace("\0", "");
                    Cardtype = "I";
                }
                else if (type == 3)
                {
                    //新外国人居住证
                    //byte[] english_name = new byte[244];
                    byte[] english_name = new byte[244];
                    byte[] sex = new byte[8];
                    byte[] id_number = new byte[64];
                    byte[] citizenship = new byte[16];
                    byte[] chinese_name = new byte[64];
                    byte[] expire_start_day = new byte[36];
                    byte[] expire_end_day = new byte[36];
                    byte[] birth_day = new byte[36];
                    byte[] version_number = new byte[12];
                    byte[] department_code = new byte[20];
                    byte[] type_sign = new byte[8];
                    byte[] reserved = new byte[16];
                    byte[] renew_count = new byte[12];
                    byte[] english_name_ex = new byte[48];
                    byte[] prev_related_info = new byte[16];
                    byte[] old_id_number = new byte[64];
                    byte[] info_buffer = new byte[64];

                    Rtn = dc_ParseTextInfoForNewForeigner(iHandle, 0, MsgLen, pMsg, chinese_name, sex, renew_count, birth_day, english_name, id_number, reserved, expire_start_day, expire_end_day, english_name_ex, citizenship, type_sign, prev_related_info, old_id_number);
                    if (Rtn != 0)
                    {
                        strResult = "-1^读取外国人信息失败";
                        dc_exit(iHandle);
                    }
                    Name = System.Text.Encoding.Default.GetString(english_name);
                    Name = Name.Replace("\0", "");
                    Sex = System.Text.Encoding.Default.GetString(sex);
                    Sex = Sex.Replace("\0", "");

                    ///国家
                    dc_ParseOtherInfo(iHandle, 2, citizenship, info_buffer);
                    Country = System.Text.Encoding.Default.GetString(info_buffer);
                    Country = Country.Replace("\0", "");

                    Birth = System.Text.Encoding.Default.GetString(birth_day);
                    Birth = Birth.Replace("\0", "");
                    Address = "";
                    CredNo = System.Text.Encoding.Default.GetString(id_number);
                    CredNo = CredNo.Replace("\0", "");
                    Department = System.Text.Encoding.Default.GetString(department_code);
                    Department = Department.Replace("\0", "");
                    Expirestartday = System.Text.Encoding.Default.GetString(expire_start_day);
                    Expirestartday = Expirestartday.Replace("\0", "");
                    Expireendday = System.Text.Encoding.Default.GetString(expire_end_day);
                    Expireendday = Expireendday.Replace("\0", "");
                    ChineseName = System.Text.Encoding.Default.GetString(chinese_name);
                    ChineseName = ChineseName.Replace("\0", "");
                    EnglishName = System.Text.Encoding.Default.GetString(english_name);
                    EnglishName = EnglishName.Replace("\0", "");
                    ///新外国人
                    Cardtype = "Y";
                }
                //姓名:中文姓名:英文姓名:性别代码:性别描述:出生日期:民族代码:民族描述:证件类型(I:外国人旧版,J:港澳台,Y:外国人新版,其他为普通身份证):证件号码:住址:年龄
                ///国籍:通行证号:签发次数:证件版本号:旧版证件号码:签发机关:签发日期:终止日期:头像照片base64编码
                String XMLData = "";
                XMLData = "<Root>";

                XMLData += FormatXMLNode("Name", Name);
                XMLData += FormatXMLNode("PeopleChineseName", ChineseName);
                XMLData += FormatXMLNode("PeopleEnglishName", EnglishName);
                XMLData += FormatXMLNode("Sex", Sex);
                XMLData += FormatXMLNode("Birth", Birth);
                XMLData += FormatXMLNode("NationDescLookUpRowID", NationDescLookUpRowID);
                XMLData += FormatXMLNode("NationDesc", NationDescLookUpRowID);
                XMLData += FormatXMLNode("Cardtype", Cardtype);

                XMLData += FormatXMLNode("CredNo", CredNo);
                XMLData += FormatXMLNode("Address", Address);
                XMLData += FormatXMLNode("Country", Country);
                XMLData += FormatXMLNode("PassCheckID", "");
                XMLData += FormatXMLNode("IssuesNum", "");
                XMLData += FormatXMLNode("PassCheckID", "");
                XMLData += FormatXMLNode("CertVersion", "");
                XMLData += FormatXMLNode("OldCredNo", "");
                XMLData += FormatXMLNode("Department", "");
                XMLData += FormatXMLNode("RegisterPlace", Address);
                XMLData += FormatXMLNode("StartDate", Expirestartday);
                XMLData += FormatXMLNode("EndDate", Expireendday);



                string PhotoPath = "C:\\DCPhoto";
                if (!Directory.Exists(PhotoPath))
                {
                    Directory.CreateDirectory(PhotoPath);
                }
                PhotoPath += "\\" + CredNo + ".bmp";
                byte[] pPhotoInfo = System.Text.Encoding.Default.GetBytes(PhotoPath);
                byte[] pPhotoBase64 = new byte[65536];
                iRet = dc_ParsePhotoInfo(iHandle, 2, PhotoLen, ref pPhoto[0], out Base64Len, out pPhotoBase64[0]);
                if (iRet == 0)
                {
                    PhotoBase64 = System.Text.Encoding.Default.GetString(pPhotoBase64);
                    PhotoBase64 = PhotoBase64.Replace("\0", "");
                    XMLData += FormatXMLNode("PhotoInfo", PhotoBase64);
                }
                XMLData += "</Root>";

                if (DetailFlag)
                {
                    strResult = "0^" + XMLData;
                }
                else
                {
                    strResult = "0^" + CredNo + "^^" + XMLData;
                }
                dc_exit(iHandle);
            }
            else
            {
                strResult = "-1^初始化设备失败,请检查读卡器设备是否连接";
            }
            return strResult;
        }

        public string ReadSSCard(Boolean DetailFlag)
        {
            string strResult = "";
            int Rtn = -1;
            int iHandle = 0;
            for (int iPort = 100; iPort < 200; iPort++)
            {
                iHandle = dc_init(iPort, 0);
                if (iHandle > 0) break;
            }
            if (iHandle > 0)
            {
                dc_beep(iHandle, 20);
                String Card_code = "";
                String Card_type = "";
                String Version = "";
                String Init_org_number = "";
                String Card_issue_date = "";
                String Card_expire_day = "";
                String Card_number = "";
                String Social_security_number = "";
                String Name = "";
                String Name_ex = "";
                String Sex = "";
                String NationDescLookUpRowID = "";
                String Birth = "";
                String Address = "";

                byte[] card_code = new byte[64];
                byte[] card_type = new byte[64];
                byte[] version = new byte[8];
                byte[] init_org_number = new byte[64];
                byte[] card_issue_date = new byte[64];
                byte[] card_expire_day = new byte[64];
                byte[] card_number = new byte[64];
                byte[] social_security_number = new byte[64];
                byte[] name = new byte[64];
                byte[] name_ex = new byte[64];
                byte[] sex = new byte[8];
                byte[] nation = new byte[12];
                byte[] birth_place = new byte[144];
                byte[] birth_day = new byte[36];

                Rtn = dc_GetSocialSecurityCardBaseInfo(iHandle, 1, card_code, card_type, version, init_org_number, card_issue_date, card_expire_day, card_number, social_security_number, name, name_ex, sex, nation, birth_place, birth_day);
                if (Rtn != 0)
                {
                    strResult = "-1^读取社保卡信息失败";
                    dc_exit(iHandle);
                }
                Card_code = System.Text.Encoding.Default.GetString(card_code);
                Card_code = Card_code.Replace("\0", "");
                Card_type = System.Text.Encoding.Default.GetString(card_type);
                Card_type = Card_type.Replace("\0", "");
                Version = System.Text.Encoding.Default.GetString(version);
                Version = Version.Replace("\0", "");
                Init_org_number = System.Text.Encoding.Default.GetString(init_org_number);
                Init_org_number = Init_org_number.Replace("\0", "");
                Card_issue_date = System.Text.Encoding.Default.GetString(card_issue_date);
                Card_issue_date = Card_issue_date.Replace("\0", "");
                Card_expire_day = System.Text.Encoding.Default.GetString(card_expire_day);
                Card_expire_day = Card_expire_day.Replace("\0", "");
                Card_number = System.Text.Encoding.Default.GetString(card_number);
                Card_number = Card_number.Replace("\0", "");
                Social_security_number = System.Text.Encoding.Default.GetString(social_security_number);
                Social_security_number = Social_security_number.Replace("\0", "");
                Name = System.Text.Encoding.Default.GetString(name);
                Name = Name.Replace("\0", "");
                Name_ex = System.Text.Encoding.Default.GetString(name_ex);
                Name_ex = Name_ex.Replace("\0", "");
                Sex = System.Text.Encoding.Default.GetString(sex);
                Sex = Sex.Replace("\0", "");
                NationDescLookUpRowID = System.Text.Encoding.Default.GetString(nation);
                NationDescLookUpRowID = NationDescLookUpRowID.Replace("\0", "");
                NationDescLookUpRowID = Convert.ToInt32(NationDescLookUpRowID).ToString();
                Birth = System.Text.Encoding.Default.GetString(birth_day);
                Birth = Birth.Replace("\0", "");
                Address = System.Text.Encoding.Default.GetString(birth_place);
                Address = Address.Replace("\0", "");

                String Cardtype = "";

                String XMLData = "";
                XMLData = "<Root>";

                XMLData += FormatXMLNode("Name", Name);
                XMLData += FormatXMLNode("Name_ex", Name_ex);
                XMLData += FormatXMLNode("Sex", Sex);
                XMLData += FormatXMLNode("Birth", Birth);
                XMLData += FormatXMLNode("NationDescLookUpRowID", NationDescLookUpRowID);
                XMLData += FormatXMLNode("NationDesc", NationDescLookUpRowID);
                XMLData += FormatXMLNode("Cardtype", Cardtype);

                XMLData += FormatXMLNode("CredNo", Social_security_number);
                XMLData += FormatXMLNode("Address", Address);
                XMLData += FormatXMLNode("PassCheckID", "");
                XMLData += FormatXMLNode("IssuesNum", "");
                XMLData += FormatXMLNode("PassCheckID", "");
                XMLData += FormatXMLNode("CertVersion", "");
                XMLData += FormatXMLNode("OldCredNo", "");
                XMLData += FormatXMLNode("Department", "");
                XMLData += FormatXMLNode("RegisterPlace", Address);
                XMLData += FormatXMLNode("StartDate", Card_issue_date);
                XMLData += FormatXMLNode("EndDate", Card_expire_day);




                XMLData += "</Root>";

                if (DetailFlag)
                {
                    strResult = "0^" + XMLData;
                }
                else
                {
                    strResult = "0^" + Social_security_number + "^^" + XMLData;
                }
                dc_exit(iHandle);
            }
            else
            {
                strResult = "-1^初始化设备失败,请检查读卡器设备是否连接";
            }
            return strResult;
        }
        public string FormatXMLNode(string Name, string Data)
        {
            return "<" + Name + ">" + Data + "</" + Name + ">";
        }
        public string ReadPersonInfo()
        {

            string strResult = ReadIDCard(true);

            return strResult;
        }
        public string ReadMagCard()
        {

            string strResult = ReadIDCard(false);

            return strResult;
        }
        public string ReadSSPersonInfo()
        {

            string strResult = ReadSSCard(true);

            return strResult;
        }
        public string ReadSSMagCard()
        {

            string strResult = ReadSSCard(false);

            return strResult;
        }
    }
}
