namespace AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

/// <summary>
/// Модули файлов docker-compose.
/// </summary>
public enum DockerComposeFileModules
{
    /// <summary>
    /// Модуль сервера (само приложение ASP.NET API).
    /// </summary>
    Server = 0,
    
    /// <summary>
    /// Модуль клиента (если для приложения будет создано фронтенд-приложение).
    /// </summary>
    Client = 1,
    
    /// <summary>
    /// Модуль Adminer для обращения к базам данных внутри docker-compose.
    /// </summary>
    Adminer = 2,
    
    /// <summary>
    /// Модуль базы данных MongoDB.
    /// </summary>
    MongoDb = 3,
    
    /// <summary>
    /// Модуль MongoExpress для обращения к MongoDb внутри docker-compose.
    /// </summary>
    MongoExpress = 4,
    
    /// <summary>
    /// Модуль базы данных ClickHouse.
    /// </summary>
    ClickHouse = 5,
    
    /// <summary>
    /// Модуль базы данных MySql.
    /// </summary>
    MySql = 6,
    
    /// <summary>
    /// Модуль базы данных Redis.
    /// </summary>
    Redis = 7,
    
    /// <summary>
    /// Модуль базы данных Elasticsearch.
    /// </summary>
    Elasticsearch = 8,
    
    /// <summary>
    /// Модуль Kibana для обращения к Elasticsearch внутри docker-compose.
    /// </summary>
    Kibana = 9,
    
    /// <summary>
    /// Модуль базы данных Cassandra.
    /// </summary>
    Cassandra = 10,
    
    /// <summary>
    /// Модуль брокера распределенных сообщений RabbitMq.
    /// </summary>
    RabbitMq = 11,
    
    /// <summary>
    /// Модуль системы мониторинга Prometheus.
    /// </summary>
    Prometheus = 12,
    
    /// <summary>
    /// Модуль системы мониторинга Grafana.
    /// </summary>
    Grafana = 13,
    
    /// <summary>
    /// Модуль веб-сервера Nginx.
    /// </summary>
    Nginx = 14,
    
    /// <summary>
    /// Модуль базы данных PostgreSql.
    /// </summary>
    PostgreSql = 15
}