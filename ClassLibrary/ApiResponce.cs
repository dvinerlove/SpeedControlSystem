using Newtonsoft.Json;

namespace ClassLibrary
{
   
    public class ApiResponce
    {
        public int Count { get; set; }
        public List<string> Data { get; set; }
        public string StateMessage { get; set; }
        public ResponceState ResponceState { get; set; }

        public ApiResponce(List<string> list)
        {
            Count = list.Count;

            if (Count>0)
            {
                ResponceState = ResponceState.Success;
                Data = list;
            }
            else
            {
                ResponceState = ResponceState.Empty;
            }
            SetStateMessage();
        }
        public ApiResponce(ResponceState state = ResponceState.ServiceUnavailable)
        {

            ResponceState = state;
            SetStateMessage();
        }
        public ApiResponce()
        {

        }

        private void SetStateMessage()
        {
            switch (ResponceState)
            {
                case ResponceState.ServiceUnavailable:
                    StateMessage = "Service unavailable at this time";
                    break;
                case ResponceState.Empty:
                    StateMessage = "Search result is empty";
                    break;
                case ResponceState.AlreadyExists:
                    StateMessage = "Item already exists";
                    break;
                case ResponceState.Success:
                    StateMessage = "Success";
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
