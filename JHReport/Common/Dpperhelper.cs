using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Dpperhelper 的摘要说明
/// </summary>
public class Dpperhelper
{
    private static readonly string mysqlconnection = System.Configuration.ConfigurationManager.ConnectionStrings["mesMyConn"].ToString();
    private static readonly string sqlconnection = System.Configuration.ConfigurationManager.ConnectionStrings["mesConn"].ToString();
    public Dpperhelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    //获取MySql的连接数据库对象。MySqlConnection
    public static IDbConnection OpenConnection()
    {
        MySqlConnection connection = new MySqlConnection(mysqlconnection);
        connection.Open();
        return connection;
    }

    //获取SqlServer的连接数据库对象。SqlServerConnection
    public static IDbConnection OpenSqlConnection()
    {
        IDbConnection connection = new SqlConnection(sqlconnection);
        connection.Open();
        return connection;
    }
}