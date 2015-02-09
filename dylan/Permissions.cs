using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunset
{
    class Permissions
    {
        //資料項目
        public static string 課程資料項目 { get { return "2c70625c-7f60-4456-a164-27dacfc32dee"; } }
        public static string 排課資料項目 { get { return "e562e1a4-8c28-45b5-ae54-aed3acba7ade"; } }
        public static string 課程分段預設值資料項目 { get { return "19e95cbd-b591-4825-8616-07ce9e463d0b"; } }
        public static string 課程分段資料項目 { get { return "713b6da0-6f44-4544-9a9e-a976ee771270"; } }

        //RibbonBar
        public static string 複製課程回ischool { get { return "0d48f433-27f7-44fa-8a43-303ae712179d"; } }
        public static bool 複製課程回ischool權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[複製課程回ischool].Executable;
            }
        }

        public static string 批次指定課程不開放查詢 { get { return "981f4a5a-7300-4e65-b7a8-8d33bbc15f02"; } }
        public static bool 批次指定課程不開放查詢權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[批次指定課程不開放查詢].Executable;
            }
        }

        public static string 批次指定課程分割設定 { get { return "f4fc1e4b-38c5-450f-9204-648a28f65688"; } }
        public static bool 批次指定課程分割設定權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[批次指定課程分割設定].Executable;
            }
        }

        public static string 刪除課程 { get { return "54da60ec-e032-4e5b-abd0-f7f1060d734b"; } }
        public static bool 刪除課程權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[刪除課程].Executable;
            }
        }

        public static string 新增課程 { get { return "85b8069c-0be0-464f-8575-ec283ddf1747"; } }
        public static bool 新增課程權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[新增課程].Executable;
            }
        }

        public static string 批次產生課程分段 { get { return "215efe71-a007-4499-ac95-2f19cef35a8d"; } }
        public static bool 批次產生課程分段權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[批次產生課程分段].Executable;
            }
        }

        public static string 教師管理 { get { return "ce016eb0-2777-40f2-92f8-fa804ce7f3a3"; } }
        public static bool 教師管理權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[教師管理].Executable;
            }
        }

        public static string 班級管理 { get { return "1fb0a1ec-d0e1-4a86-b984-e5fdf1909937"; } }
        public static bool 班級管理權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[班級管理].Executable;
            }
        }

        public static string 場地管理 { get { return "84b4232c-0d45-4f36-8af7-466f7a34011f"; } }
        public static bool 場地管理權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[場地管理].Executable;
            }
        }

        public static string 時間表管理 { get { return "f9203ed6-bb08-4ae0-a55d-f780dda52287"; } }
        public static bool 時間表管理權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[時間表管理].Executable;
            }
        }

        public static string 指定課程預設場地 { get { return "bbc725fb-6c4a-47e9-924e-800fdc3a1ea1"; } }
        public static bool 指定課程預設場地權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[指定課程預設場地].Executable;
            }
        }

        public static string 指定課程時間表 { get { return "25f989b6-c570-4ab6-b721-1876634c652e"; } }
        public static bool 指定課程時間表權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[指定課程時間表].Executable;
            }
        }

        public static string 匯出程分段資料 { get { return "61e0cba4-ca4a-45b1-a41b-d1bcb8e91493"; } }
        public static bool 匯出程分段資料權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[匯出程分段資料].Executable;
            }
        }

        public static string 匯出課程資料 { get { return "04c94a95-61fc-43e8-add5-c629767b082e"; } }
        public static bool 匯出課程資料權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[匯出課程資料].Executable;
            }
        }

        public static string 匯入課程分段資料 { get { return "9006b53c-e767-4ab1-b82a-d538797989b4"; } }
        public static bool 匯入課程分段資料權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[匯入課程分段資料].Executable;
            }
        }

        public static string 匯入課程資料 { get { return "ec6e8802-a677-45e0-8fcd-5e113731c2d6"; } }
        public static bool 匯入課程資料權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[匯入課程資料].Executable;
            }
        }

        public static string 依課程規劃表開課 { get { return "95087c63-92a9-4dc2-b951-a028a41d964d"; } }
        public static bool 依課程規劃表開課權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[依課程規劃表開課].Executable;
            }
        }

        public static string 複製課程到其他學期 { get { return "b360fe40-1af3-4a79-b745-1ea11e1dce10"; } }
        public static bool 複製課程到其他學期權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[複製課程到其他學期].Executable;
            }
        }

        public static string 課程規劃表 { get { return "d1d959a6-50e6-46f5-ac0f-7b2264ece9ee"; } }
        public static bool 課程規劃表權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[課程規劃表].Executable;
            }
        }

        public static string 重設國高中課規狀態 { get { return "edy77864-35yu-658u-as88-2d552drer8cc"; } }
        public static bool 重設國高中課規狀態權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[重設國高中課規狀態].Executable;
            }
        }

        public static string 班級教師檢查 { get { return "6b33a133-7bef-4c47-8100-319684458846"; } }
        public static bool 班級教師檢查權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[班級教師檢查].Executable;
            }
        }

        public static string 學生功課表 { get { return "Sunset.dylan.Report.HomeworkTableForm.1020820"; } }
        public static bool 學生功課表權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[學生功課表].Executable;
            }
        }
    }
}
