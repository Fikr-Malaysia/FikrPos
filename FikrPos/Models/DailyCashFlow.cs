using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FikrPos.Models
{
    public class DailyCashFlow
    {
        public string branch_name;
        public string branch_token;
        public string company_token;

        public class CashFlowData
        {
            public string day; //todo : DATE
            public int cash_start_of_day;
            public int cash_end_of_day;
        }

        public CashFlowData data;
    }
}
