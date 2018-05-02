
namespace Epine.Domain
{

    /// <summary>
    ///		Defines Output types for ETL Data.
    /// </summary>
    public enum DataOutputType : int
    {

        /// <summary>
        ///		Jupiter Queue Output
        /// </summary>
        Jupiter = 1,

        /// <summary>
        ///		Web Service Output
        /// </summary>
        Service = 2,

        /// <summary>
        ///		FTP File Output
        /// </summary>
        FTP = 3,

        /// <summary>
        ///		MQ Queue Output
        /// </summary>
        Queue = 4,

        /// <summary>
        ///		SMS Service Output
        /// </summary>
        Sms = 5,

        /// <summary>
        ///		Sql/MySql Data Output
        /// </summary>
        Database = 6,

        /// <summary>
        ///		PDF File Output
        /// </summary>
        Pdf = 7,

        /// <summary>
        ///		Email Summary Output
        /// </summary>
        EmailSummary = 8,

        /// <summary>
        ///		COJ Output
        /// </summary>
        CityOfJoburg = 9,

        /// <summary>
        ///		Jupiter Email Queue Output
        /// </summary>
        JupiterEmail = 10,

        /// <summary>
        ///		Jupiter Email Queue Output
        /// </summary>
        SubProcess = 11,

        /// <summary>
        ///		Netcare Delivery Report Output
        /// </summary>
        NetcareCollection = 12,

        /// <summary>
        ///		Netcare Delivery Report Output
        /// </summary>
        JupiterSMS = 13,
        /// <summary>
		///		Netcare Delivery Report Output
		/// </summary>
		JupiterCallback = 14,

        /// <summary>
        ///		Netcare Delivery Report Output
        /// </summary>
        JupiterAepVideo = 15,

        /// <summary>
        ///		Storage Output
        /// </summary>
        Storage = 16,

        /// <summary>
        ///		Packet Transfer 17
        /// </summary>
        PacketTransfer = 17,
        /// <summary>
		///		Hollard Report Output
		/// </summary>
        HollardMessage = 18,

        /// <summary>
        ///		Scrub Summary Report Output
        /// </summary>
        ScrubSummary = 19,

        /// <summary>
        ///		Email Statistic Database Output
        /// </summary>
        Statistic = 20,

        /// <summary>
        ///		Optimove Delivery and Reply Output
        /// </summary>
        Optimove = 21,
        /// <summary>
        ///		Optimove Delivery and Reply Output
        /// </summary>
        MultichoiceCrmDeliveryReport = 22
    }
}

