using System.ComponentModel;

namespace Epine.Infrastructure.Resolvers {

	/// <summary>
	/// 
	/// </summary>
	public enum TokenValue {

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.EPOCH_1DAY)
		]
		Epoch_1Day = 1,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.EPOCH_2DAY)
		]
		Epoch_2Day = 2,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.EPOCH_3DAY)
		]
		Epoch_3Day = 3,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.GUID_D)
		]
		GUID_D = 4,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.GUID_N)
		]
		GUID_N = 5,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.DATE_NOW)
		]
		Now = 6,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.ONE_TIME_PIN)
		]
		OneTimePin = 7,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.CURRENT_MONTH_YEAR)
		]
		CurrentMonthYear = 8,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.NEXT_MONTH_YEAR)
		]
		NextMonthYear = 9,

		/// <summary>
		/// 
		/// </summary>
		[field:
			Description(TokenValueResolver.FIRST_DAY_WORK_NEXT_MONTH)
		]
		FIRST_DAY_WORK_NEXT_MONTH = 10,

        /// <summary>
		/// 
		/// </summary>
		[field:
            Description(TokenValueResolver.FIRST_DAY_NEXT_MONTH)
        ]
        FIRST_DAY_NEXT_MONTH = 11,

        /// <summary>
		/// 
		/// </summary>
		[field:
            Description(TokenValueResolver.IS_SUNDAY)
        ]
        IS_SUNDAY = 12,

        /// <summary>
        /// 
        /// </summary>
        [field:
            Description(TokenValueResolver.FIXED_LENGTH_STRING)
        ]
        FIXED_LENGTH_STRING = 13,
    }
}
