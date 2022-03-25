using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClassLibrary
{
    public enum ResponceState
    {
        Success,
        Error,
        Empty
    }
    public class Responce
    {
        public Responce(List<string> list)
        {
            Count = list.Count;

            if (Count>0)
            {
                ResponceState = ResponceState.Success;
                Data = list;
                StateMessage = "Success";
            }
            else
            {
                ResponceState = ResponceState.Empty;
                StateMessage = "Search result is empty";
            }
        }
        public Responce()
        {
            ResponceState = ResponceState.Error;
            StateMessage = "Service unavailable at this time";
        }

        public int Count { get; set; }
        public object Data { get; set; }
        public object StateMessage { get; set; }
        public ResponceState ResponceState { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
