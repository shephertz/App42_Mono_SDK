using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.user;
using com.shephertz.app42.paas.sdk.csharp.review;
using com.shephertz.app42.paas.sdk.csharp.upload;
using com.shephertz.app42.paas.sdk.csharp.session;
using com.shephertz.app42.paas.sdk.csharp.recommend;
using com.shephertz.app42.paas.sdk.csharp.log;
using com.shephertz.app42.paas.sdk.csharp.gallery;
using com.shephertz.app42.paas.sdk.csharp.game;
using com.shephertz.app42.paas.sdk.csharp.reward;
using com.shephertz.app42.paas.sdk.csharp.shopping;
using com.shephertz.app42.paas.sdk.csharp.message;
using com.shephertz.app42.paas.sdk.csharp.imageProcessor;
using com.shephertz.app42.paas.sdk.csharp.email;
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp.geo;
using com.shephertz.app42.paas.sdk.csharp.social;
using com.shephertz.app42.paas.sdk.csharp.pushNotification;
using com.shephertz.app42.paas.sdk.csharp.customcode;


namespace com.shephertz.app42.paas.sdk.csharp
{
    /// <summary>
    /// The ServiceAPI class is used to build the Service objects which are used to call various api methods.
    /// @author Ajay Tiwari
    /// </summary>
    public class ServiceAPI
    {
        private String apiKey;
        private String secretKey;
        private Config config = Config.GetInstance();
        String baseURL;
        /// <summary>
        /// This is a constructor that takes
        /// </summary>
        /// <param name="apiKey">Key used in conjunction with secretKey for authentication.</param>
        /// <param name="secretKey">Key used in conjunction with apiKey for authentication.</param>
        public ServiceAPI(String apiKey, String secretKey)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }
        /// <summary>
        /// Retrieve the value of apiKey.
        /// </summary>
        /// <returns>A variable of String type</returns>
        public String GetApiKey()
        {
            return apiKey;
        }
        /// <summary>
        /// Sets the value of apiKey
        /// </summary>
        /// <param name="apiKey">A variable of String type.</param>
        public void SetApiKey(String apiKey)
        {
            this.apiKey = apiKey;
        }
        /// <summary>
        /// Retrieve the value of secretKey.
        /// </summary>
        /// <returns>A variable of String type.</returns>
        public String GetSecretKey()
        {
            return secretKey;
        }
        /// <summary>
        /// Sets the value of secretKey
        /// </summary>
        /// <param name="secretKey">A variable of String type.</param>
        public void SetSecretKey(String secretKey)
        {
            this.secretKey = secretKey;
        }
        /// <summary>
        /// Retrieve the value of baseURL.
        /// </summary>
        /// <returns>A variable of String type.</returns>
        public String GetBaseURL()
        {
            return config.GetBaseURL();
        }
        /// <summary>
        /// Sets the value of Config.baseURL
        /// </summary>
        /// <param name="protocol">A variable of String type.</param>
        /// <param name="host">A variable of String type.</param>
        /// <param name="port">A variable of Integer type.</param>
        public void SetBaseURL(String protocol, String host, Int32 port)
        {
            config.SetBaseURL(protocol, host, port);
            this.baseURL = config.GetBaseURL();
        }
        /// <summary>
        /// Sets the value of Config.customCodeURL
        /// </summary>
        /// <param name="url">A variable of String type.</param>

        public void SetCustomCodeURL(String url)
        {
            config.SetCustomCodeURL(url);
        }

        /// <summary>
        /// Builds the instance of UserService.
        /// </summary>
        /// <returns>UserService Object</returns>
        /// <see cref="">UserService</see>
        public UserService BuildUserService()
        {
            UserService userService = new UserService(apiKey, secretKey);
            return userService;
        }
        /// <summary>
        /// Builds the instance of ReviewService.
        /// </summary>
        /// <returns>ReviewService Object</returns>
        /// <see cref="">ReviewService</see>
        public ReviewService BuildReviewService()
        {
            ReviewService reviewService = new ReviewService(apiKey, secretKey, baseURL);
            return reviewService;
        }
        /// <summary>
        /// Builds the instance of UploadService.
        /// </summary>
        /// <returns>UploadService Object</returns>
        /// <see cref="">UploadService</see>
        public UploadService BuildUploadService()
        {
            UploadService uploadService = new UploadService(apiKey, secretKey);
            return uploadService;
        }
        /// <summary>
        /// Builds the instance of SessionService.
        /// </summary>
        /// <returns>SessionService Object</returns>
        /// <see cref="">SessionService</see>
        public SessionService BuildSessionService()
        {
            SessionService sessionService = new SessionService(apiKey, secretKey);
            return sessionService;
        }
        /// <summary>
        /// Builds the instance of RecommenderService.
        /// </summary>
        /// <returns>RecommenderService Object</returns>
        /// <see cref="">RecommenderService</see>
        public RecommenderService BuildRecommenderService()
        {
            RecommenderService recommenderService = new RecommenderService(apiKey, secretKey);
            return recommenderService;
        }
        /// <summary>
        /// Builds the instance of LogService.
        /// </summary>
        /// <returns>LogService Object</returns>
        /// <see cref="">LogService</see>
        public LogService BuildLogService()
        {
            LogService logService = new LogService(apiKey, secretKey);
            return logService;
        }
        /// <summary>
        /// Builds the instance of PhotoService.
        /// </summary>
        /// <returns>PhotoService Object</returns>
        /// <see cref="">PhotoService</see>
        public PhotoService BuildPhotoService()
        {
            PhotoService photoService = new PhotoService(apiKey, secretKey, baseURL);
            return photoService;
        }
        /// <summary>
        /// Builds the instance of AlbumService.
        /// </summary>
        /// <returns>AlbumService Object</returns>
        /// <see cref="">AlbumService</see>
        public AlbumService BuildAlbumService()
        {
            AlbumService albumService = new AlbumService(apiKey, secretKey, baseURL);
            return albumService;
        }
        /// <summary>
        /// Builds the instance of GameService.
        /// </summary>
        /// <returns>GameService Object</returns>
        /// <see cref="">GameService</see>
        public GameService BuildGameService()
        {
            GameService gameService = new GameService(apiKey, secretKey, baseURL);
            return gameService;
        }
        /// <summary>
        /// Builds the instance of RewardService.
        /// </summary>
        /// <returns>RewardService Object</returns>
        /// <see cref="">RewardService</see>
        public RewardService BuildRewardService()
        {
            RewardService rewardService = new RewardService(apiKey, secretKey, baseURL);
            return rewardService;
        }
        /// <summary>
        /// Builds the instance of ScoreService.
        /// </summary>
        /// <returns>ScoreService Object</returns>
        /// <see cref="">ScoreService</see>
        public ScoreService BuildScoreService()
        {
            ScoreService scoreService = new ScoreService(apiKey, secretKey, baseURL);
            return scoreService;
        }
        /// <summary>
        /// Builds the instance of ScoreBoardService.
        /// </summary>
        /// <returns>ScoreBoardService Object</returns>
        /// <see cref="">ScoreBoardService</see>
        public ScoreBoardService BuildScoreBoardService()
        {
            ScoreBoardService scoreBoardService = new ScoreBoardService(apiKey, secretKey, baseURL);
            return scoreBoardService;
        }
        /// <summary>
        /// Builds the instance of CartService.
        /// </summary>
        /// <returns>CartService Object</returns>
        /// <see cref="">CartService</see>
        public CartService BuildCartService()
        {
            CartService cartService = new CartService(apiKey, secretKey, baseURL);
            return cartService;
        }
        /// <summary>
        /// Builds the instance of CatalogueService.
        /// </summary>
        /// <returns>CatalogueService Object</returns>
        /// <see cref="">CatalogueService</see>
        public CatalogueService BuildCatalogueService()
        {
            CatalogueService catalogueService = new CatalogueService(apiKey, secretKey, baseURL);
            return catalogueService;
        }
        /// <summary>
        /// Builds the instance of Charge.
        /// </summary>
        /// <returns>Charge Object</returns>
        /// <see cref="">Charge</see>
        //public Charge BuildChargeService() {
        //    Charge chargeService = new Charge(apiKey, secretKey, config.GetBaseURL());
        //    return chargeService;
        //}
        /// <summary>
        /// Builds the instance of QueueService.
        /// </summary>
        /// <returns>QueueService Object</returns>
        /// <see cref="">QueueService</see>
        public QueueService BuildQueueService()
        {
            QueueService queueService = new QueueService(apiKey, secretKey, baseURL);
            return queueService;
        }
        /// <summary>
        /// Builds the instance of ImageProcessorService.
        /// </summary>
        /// <returns>ImageProcessorService Object</returns>
        /// <see cref="">ImageProcessorService</see>
        public ImageProcessorService BuildImageProcessorService()
        {
            ImageProcessorService imageProcessorService = new ImageProcessorService(apiKey, secretKey, baseURL);
            return imageProcessorService;
        }
        /// <summary>
        /// Builds the instance of StorageService.
        /// </summary>
        /// <returns>StorageService Object</returns>
        /// <see cref="">StorageService</see>
        public StorageService BuildStorageService()
        {
            StorageService storageService = new StorageService(apiKey, secretKey);
            return storageService;
        }
        /// <summary>
        /// Builds the instance of EmailService.
        /// </summary>
        /// <returns>EmailService Object</returns>
        /// <see cref="">EmailService</see>
        public EmailService BuildEmailService()
        {
            EmailService emailService = new EmailService(apiKey, secretKey);
            return emailService;
        }
        /// <summary>
        /// Builds the instance of GeoService.
        /// </summary>
        /// <returns>GeoService Object</returns>
        /// <see cref="">GeoService</see>
        public GeoService BuildGeoService()
        {
            GeoService geoService = new GeoService(apiKey, secretKey, baseURL);
            return geoService;
        }
        /// <summary>
        /// Builds the instance of SocialService.
        /// </summary>
        /// <returns>SocialService Object</returns>
        /// <see cref="">SocialService</see>
        public SocialService BuildSocialService()
        {
            SocialService socialService = new SocialService(apiKey, secretKey, config.GetBaseURL());
            return socialService;
        }
        /// <summary>
        /// 
        /// Builds the instance of PushNotificationService
        /// </summary>
        /// <returns>PushNotificationService </returns>
        public PushNotificationService BuildPushNotificationService()
        {
            PushNotificationService pushNotificationService = new PushNotificationService(apiKey, secretKey, config.GetBaseURL());
            return pushNotificationService;
        }

        /// <summary>
        /// 
        /// Builds the instance of CustomCodeService
        /// </summary>
        /// <returns>CustomCodeService </returns>
        public CustomCodeService BuildCustomCodeService()
        {
            CustomCodeService customCodeService = new CustomCodeService(apiKey, secretKey, config.GetBaseURL());
            return customCodeService;
        }
       
    }
}