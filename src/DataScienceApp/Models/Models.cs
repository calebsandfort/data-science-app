using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration;

namespace DataScienceApp.Models
{
    #region EntityBase
    public class EntityBase
    {
        public int ID { get; set; }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            EntityBase x = obj as EntityBase;
            if ((System.Object)x == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.ID == x.ID;
        }

        public bool Equals(EntityBase x)
        {
            // If parameter is null return false:
            if ((object)x == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.ID == x.ID;
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
    }
    #endregion

    #region Stock
    public class Stock : EntityBase
    {
        [StringLength(4)]
        public String Symbol { get; set; }

        public List<PriceEntry> Prices { get; set; }

        public void FillCalculatedFields()
        {
            PriceEntry twoDaysAgo, yesterday, today;

            for (int i = 2; i < this.Prices.Count; i++)
            {
                twoDaysAgo = this.Prices[i - 2];
                yesterday = this.Prices[i - 1];
                today = this.Prices[i];

                today.PctChange = ((Double)today.Close - (Double)yesterday.Close) / (Double)yesterday.Open;
                today.YdaPctChange = ((Double)yesterday.Close - (Double)twoDaysAgo.Close) / (Double)twoDaysAgo.Open;
            }
        }
    }
    #endregion

    #region PriceEntry
    public class PriceEntry : EntityBase
    {
        [DataType(DataType.Currency)]
        public Decimal High { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Low { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Open { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Close { get; set; }

        [DisplayFormat(DataFormatString = "{0:P4}")]
        public Double PctChange { get; set; }

        [DisplayFormat(DataFormatString = "{0:P4}")]
        public Double YdaPctChange { get; set; }

        public int Volume { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        public int StockID { get; set; }
        public Stock Stock { get; set; }
    }
    #endregion

    #region PriceEntriesUpload
    public class PriceEntriesUpload
    {
        public String Symbol { get; set; }

        [DataType(DataType.MultilineText)]
        public String Contents { get; set; }
    }
    #endregion

    #region LinearModelObservation
    public class LinearModelObservation
    {
        public Double ExplanatoryVariable { get; set; }
        public Double OutcomeVariable { get; set; }
    }

    public class LinearModelObservationMap : CsvClassMap<LinearModelObservation>
    {
        public LinearModelObservationMap()
        {
            Map(m => m.ExplanatoryVariable).Name("Explanatory.Variable");
            Map(m => m.OutcomeVariable).Name("Outcome.Variable");
        }
    }
    #endregion
}
