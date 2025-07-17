using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace HawalatySystem.Resources.Localization
{
    public class LocalizationManager
    {
        private static LocalizationManager _instance;
        private Dictionary<string, Dictionary<string, string>> _resources;
        private string _currentLanguage = "en";

        public static LocalizationManager Instance => _instance ??= new LocalizationManager();

        public event System.EventHandler LanguageChanged;

        private LocalizationManager()
        {
            InitializeResources();
        }

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(value);
                    LanguageChanged?.Invoke(this, System.EventArgs.Empty);
                }
            }
        }

        public string GetString(string key)
        {
            if (_resources.ContainsKey(_currentLanguage) && _resources[_currentLanguage].ContainsKey(key))
            {
                return _resources[_currentLanguage][key];
            }
            
            // Fallback to English
            if (_resources.ContainsKey("en") && _resources["en"].ContainsKey(key))
            {
                return _resources["en"][key];
            }

            return key; // Return key if translation not found
        }

        private void InitializeResources()
        {
            _resources = new Dictionary<string, Dictionary<string, string>>
            {
                ["en"] = new Dictionary<string, string>
                {
                    // Main Navigation
                    ["Dashboard"] = "Dashboard",
                    ["Transfers"] = "Transfers",
                    ["NewTransfer"] = "New Transfer",
                    ["Agents"] = "Agents",
                    ["Reports"] = "Reports",
                    ["Settings"] = "Settings",
                    ["Users"] = "Users",
                    ["ExchangeRates"] = "Exchange Rates",
                    ["Logout"] = "Logout",
                    
                    // Dashboard
                    ["TotalIncoming"] = "Total Incoming",
                    ["TotalOutgoing"] = "Total Outgoing",
                    ["PendingTransfers"] = "Pending Transfers",
                    ["CompletedTransfers"] = "Completed Transfers",
                    ["TotalCommission"] = "Total Commission",
                    ["DailyReport"] = "Daily Report",
                    ["WeeklyReport"] = "Weekly Report",
                    ["MonthlyReport"] = "Monthly Report",
                    
                    // Transfer Form
                    ["TransferId"] = "Transfer ID",
                    ["SenderName"] = "Sender Name",
                    ["ReceiverName"] = "Receiver Name",
                    ["FromCountry"] = "From Country",
                    ["ToCountry"] = "To Country",
                    ["Currency"] = "Currency",
                    ["Amount"] = "Amount",
                    ["Commission"] = "Commission",
                    ["FinalAmount"] = "Final Amount",
                    ["TransferMethod"] = "Transfer Method",
                    ["Status"] = "Status",
                    ["Notes"] = "Notes",
                    ["SenderPhone"] = "Sender Phone",
                    ["ReceiverPhone"] = "Receiver Phone",
                    ["ExchangeRate"] = "Exchange Rate",
                    
                    // Status
                    ["Pending"] = "Pending",
                    ["Completed"] = "Completed",
                    ["Cancelled"] = "Cancelled",
                    ["InProgress"] = "In Progress",
                    
                    // Countries
                    ["Libya"] = "Libya",
                    ["Turkey"] = "Turkey",
                    
                    // Actions
                    ["Save"] = "Save",
                    ["Cancel"] = "Cancel",
                    ["Edit"] = "Edit",
                    ["Delete"] = "Delete",
                    ["Search"] = "Search",
                    ["Filter"] = "Filter",
                    ["Export"] = "Export",
                    ["Print"] = "Print",
                    ["Complete"] = "Complete",
                    ["Add"] = "Add",
                    ["Update"] = "Update",
                    
                    // Agent Management
                    ["AgentName"] = "Agent Name",
                    ["Country"] = "Country",
                    ["City"] = "City",
                    ["Phone1"] = "Phone 1",
                    ["Phone2"] = "Phone 2",
                    ["Email"] = "Email",
                    ["Address"] = "Address",
                    ["DailyLimit"] = "Daily Limit",
                    ["CurrentBalance"] = "Current Balance",
                    ["IsActive"] = "Is Active",
                    
                    // User Management
                    ["Username"] = "Username",
                    ["FullName"] = "Full Name",
                    ["Password"] = "Password",
                    ["ConfirmPassword"] = "Confirm Password",
                    ["Role"] = "Role",
                    ["Phone"] = "Phone",
                    
                    // Roles
                    ["Admin"] = "Administrator",
                    ["Accountant"] = "Accountant",
                    ["Agent"] = "Agent",
                    ["Manager"] = "Manager",
                    ["Viewer"] = "Viewer",
                    
                    // Messages
                    ["LoginSuccessful"] = "Login successful",
                    ["LoginFailed"] = "Invalid username or password",
                    ["SaveSuccessful"] = "Data saved successfully",
                    ["SaveFailed"] = "Failed to save data",
                    ["DeleteConfirm"] = "Are you sure you want to delete this item?",
                    ["ValidationError"] = "Please check the entered data",
                    
                    // Login
                    ["Login"] = "Login",
                    ["WelcomeMessage"] = "Welcome to Hawalaty System",
                    ["SystemTitle"] = "Hawalaty System - TurkeyLibya Transfer System"
                },
                
                ["ar"] = new Dictionary<string, string>
                {
                    // Main Navigation
                    ["Dashboard"] = "لوحة التحكم",
                    ["Transfers"] = "التحويلات",
                    ["NewTransfer"] = "تحويل جديد",
                    ["Agents"] = "الوكلاء",
                    ["Reports"] = "التقارير",
                    ["Settings"] = "الإعدادات",
                    ["Users"] = "المستخدمين",
                    ["ExchangeRates"] = "أسعار الصرف",
                    ["Logout"] = "تسجيل الخروج",
                    
                    // Dashboard
                    ["TotalIncoming"] = "إجمالي الوارد",
                    ["TotalOutgoing"] = "إجمالي الصادر",
                    ["PendingTransfers"] = "التحويلات المعلقة",
                    ["CompletedTransfers"] = "التحويلات المكتملة",
                    ["TotalCommission"] = "إجمالي العمولة",
                    ["DailyReport"] = "التقرير اليومي",
                    ["WeeklyReport"] = "التقرير الأسبوعي",
                    ["MonthlyReport"] = "التقرير الشهري",
                    
                    // Transfer Form
                    ["TransferId"] = "رقم التحويل",
                    ["SenderName"] = "اسم المرسل",
                    ["ReceiverName"] = "اسم المستلم",
                    ["FromCountry"] = "من دولة",
                    ["ToCountry"] = "إلى دولة",
                    ["Currency"] = "العملة",
                    ["Amount"] = "المبلغ",
                    ["Commission"] = "العمولة",
                    ["FinalAmount"] = "المبلغ النهائي",
                    ["TransferMethod"] = "طريقة التحويل",
                    ["Status"] = "الحالة",
                    ["Notes"] = "ملاحظات",
                    ["SenderPhone"] = "هاتف المرسل",
                    ["ReceiverPhone"] = "هاتف المستلم",
                    ["ExchangeRate"] = "سعر الصرف",
                    
                    // Status
                    ["Pending"] = "معلق",
                    ["Completed"] = "مكتمل",
                    ["Cancelled"] = "ملغى",
                    ["InProgress"] = "قيد التنفيذ",
                    
                    // Countries
                    ["Libya"] = "ليبيا",
                    ["Turkey"] = "تركيا",
                    
                    // Actions
                    ["Save"] = "حفظ",
                    ["Cancel"] = "إلغاء",
                    ["Edit"] = "تعديل",
                    ["Delete"] = "حذف",
                    ["Search"] = "بحث",
                    ["Filter"] = "تصفية",
                    ["Export"] = "تصدير",
                    ["Print"] = "طباعة",
                    ["Complete"] = "إكمال",
                    ["Add"] = "إضافة",
                    ["Update"] = "تحديث",
                    
                    // Agent Management
                    ["AgentName"] = "اسم الوكيل",
                    ["Country"] = "الدولة",
                    ["City"] = "المدينة",
                    ["Phone1"] = "الهاتف الأول",
                    ["Phone2"] = "الهاتف الثاني",
                    ["Email"] = "البريد الإلكتروني",
                    ["Address"] = "العنوان",
                    ["DailyLimit"] = "الحد اليومي",
                    ["CurrentBalance"] = "الرصيد الحالي",
                    ["IsActive"] = "نشط",
                    
                    // User Management
                    ["Username"] = "اسم المستخدم",
                    ["FullName"] = "الاسم الكامل",
                    ["Password"] = "كلمة المرور",
                    ["ConfirmPassword"] = "تأكيد كلمة المرور",
                    ["Role"] = "الدور",
                    ["Phone"] = "الهاتف",
                    
                    // Roles
                    ["Admin"] = "مدير النظام",
                    ["Accountant"] = "محاسب",
                    ["Agent"] = "وكيل",
                    ["Manager"] = "مدير",
                    ["Viewer"] = "مشاهد",
                    
                    // Messages
                    ["LoginSuccessful"] = "تم تسجيل الدخول بنجاح",
                    ["LoginFailed"] = "اسم المستخدم أو كلمة المرور غير صحيحة",
                    ["SaveSuccessful"] = "تم حفظ البيانات بنجاح",
                    ["SaveFailed"] = "فشل في حفظ البيانات",
                    ["DeleteConfirm"] = "هل أنت متأكد من حذف هذا العنصر؟",
                    ["ValidationError"] = "يرجى التحقق من البيانات المدخلة",
                    
                    // Login
                    ["Login"] = "تسجيل الدخول",
                    ["WelcomeMessage"] = "مرحباً بك في نظام الحوالة",
                    ["SystemTitle"] = "نظام الحوالة - نظام التحويل بين تركيا وليبيا"
                }
            };
        }
    }
}