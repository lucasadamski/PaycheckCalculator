namespace PayrollCalculator
{
    class InvalidTimeFormatException : Exception
    {
        public string MyMessage;
        public InvalidTimeFormatException()
        {
            MyMessage = "Please provide time in a HH:MM format, eg. 13:45";
        }
    }
}


