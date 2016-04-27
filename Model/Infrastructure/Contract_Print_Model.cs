using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgroFirma.Component
{

    public class Contract_Print_Model
    {
        /// <summary>
        /// Номер договора
        /// {ID}
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Город компании
        /// {NAME_COMPANY_CITY}
        /// </summary>
        public string NAME_COMPANY_CITY { get; set; }

        /// <summary>
        /// Дата регистрации
        /// {DATE_REG}
        /// </summary>
        public string DATE_REG { get; set; }

        /// <summary>
        /// Наименование компании поставщика
        /// {NAME_COMPANY_CONTRACTOR}
        /// </summary>
        public string NAME_COMPANY_CONTRACTOR { get; set; }

        /// <summary>
        /// ФИО поставщика
        /// {FIO_CONTRACTOR}
        /// </summary>
        public string FIO_CONTRACTOR { get; set; }

        /// <summary>
        /// Наименование компании заказчика
        /// {NAME_COMPANY_USER}
        /// </summary>
        public string NAME_COMPANY_USER { get; set; }

        /// <summary>
        /// ФИО Заказчика
        /// {FIO_USER}
        /// </summary>
        public string FIO_USER { get; set; }

        /// <summary>
        /// Юридический адресс поставщика
        /// {LEGAL_ADDRESS_CONTRACTOR}
        /// </summary>
        public string LEGAL_ADDRESS_CONTRACTOR { get; set; }

        /// <summary>
        /// Юридический адресс заказчика
        /// {LEGAL_ADDRESS_USER}
        /// </summary>
        public string LEGAL_ADDRESS_USER { get; set; }

        /// <summary>
        /// Почтовый адресс поставщика
        /// {MAIL_ADDRESS_CONTRACTOR}
        /// </summary>
        public string MAIL_ADDRESS_CONTRACTOR { get; set; }

        /// <summary>
        /// Почтовый адресс заказчика
        /// {MAIL_ADDRESS_USER}
        /// </summary>
        public string MAIL_ADDRESS_USER { get; set; }

        /// <summary>
        /// Телефон поставщика
        /// {PHONE_CONTRACTOR}
        /// </summary>
        public string PHONE_CONTRACTOR { get; set; }

        /// <summary>
        /// Факс поставщика
        /// {FAX_CONTRACTOR}
        /// </summary>
        public string FAX_CONTRACTOR { get; set; }

        /// <summary>
        /// Телефон заказчика
        /// {PHONE_USER}
        /// </summary>
        public string PHONE_USER { get; set; }

        /// <summary>
        /// Факс заказчика
        /// {FAX_USER}
        /// </summary>
        public string FAX_USER { get; set; }

        /// <summary>
        /// ИНН поставщика
        /// {INN_CONTRACTOR}
        /// </summary>
        public string INN_CONTRACTOR { get; set; }

        /// <summary>
        /// ИНН заказчика
        /// {INN_USER}
        /// </summary>
        public string INN_USER { get; set; }

        /// <summary>
        /// КПП поставщика
        /// {KPP_CONTRACTOR}
        /// </summary>
        public string KPP_CONTRACTOR { get; set; }

        /// <summary>
        /// КПП заказчика
        /// {KPP_USER}
        /// </summary>
        public string KPP_USER { get; set; }

        /// <summary>
        /// Расчётный счёт поставщика
        /// {CHECKING_ACCOUNT_CONTRACTOR}
        /// </summary>
        public string CHECKING_ACCOUNT_CONTRACTOR { get; set; }

        /// <summary>
        /// Расчётный счёт заказчика
        /// {CHECKING_ACCOUNT_USER}
        /// </summary>
        public string CHECKING_ACCOUNT_USER { get; set; }

        /// <summary>
        /// Банк поставщика
        /// {BANK_CONTRACTOR}
        /// </summary>
        public string BANK_CONTRACTOR { get; set; }

        /// <summary>
        /// Банк зказчика
        /// {BANK_USER}
        /// </summary>
        public string BANK_USER { get; set; }
        
        /// <summary>
        /// Корреспондентский счёт поставщика
        /// {CORRESPONDENT_ACCOUNT_CONTRACTOR}
        /// </summary>
        public string CORRESPONDENT_ACCOUNT_CONTRACTOR { get; set; }

        /// <summary>
        /// Корреспондентский счёт заказчика
        /// {CORRESPONDENT_ACCOUNT_USER}
        /// </summary>
        public string CORRESPONDENT_ACCOUNT_USER { get; set; }

        /// <summary>
        /// БИК поставщика
        /// {BIK_CONTRACTOR}
        /// </summary>
        public string BIK_CONTRACTOR { get; set; }

        /// <summary>
        /// БИК пользователя
        /// {BIK_USER}
        /// </summary>
        public string BIK_USER { get; set; }
    }
}