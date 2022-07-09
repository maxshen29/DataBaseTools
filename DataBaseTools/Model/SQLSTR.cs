using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTools.Model
{
    public class SQLSTR
    {
	
		/// <summary>
		/// SqlServerProperties----------
		/// </summary>
		public static string _sqlstrSqlServerProperties = @"Declare @AuditLevel int,@AuditLevelDesc varchar(50) 
	exec master..xp_instance_regread @rootkey='HKEY_LOCAL_MACHINE', @key='SOFTWARE\Microsoft\MSSQLServer\MSSQLServer', @value_name='AuditLevel', @value=@AuditLevel output
	SET @AuditLevelDesc=CASE @AuditLevel WHEN 0 THEN 'None' WHEN 1 THEN 'Successful Logins Only' WHEN 2 THEN 'Failed Logins Only' WHEN 3 THEN 'Both Failed and Successful Logins' END
	SELECT @@SERVERNAME ServerName, @@VERSION InstanceVersion,SERVERPROPERTY('ProductVersion') AS [ProductVersion],SERVERPROPERTY('ProductLevel') AS [ProductLevel],SERVERPROPERTY('Edition') AS [Edition],SERVERPROPERTY('BuildClrVersion') AS [ClrVersion],
	SERVERPROPERTY('Collation') AS [Collation],CASE SERVERPROPERTY('IsIntegratedSecurityOnly') WHEN 1 THEN 'Windows' ELSE 'Mixed' END [AuthenticationMode],SERVERPROPERTY('ComputerNamePhysicalNetBIOS') AS [SqlHost],COALESCE(SERVERPROPERTY('InstanceName'), 'MSSQLSERVER') AS [InstanceName],
	SERVERPROPERTY('IsClustered') AS [IsClustered],SERVERPROPERTY('IsFullTextInstalled') AS [IsFullTextInstalled],SERVERPROPERTY('IsSingleUser') AS [IsSingleUser],@AuditLevelDesc LoginAuditing,SERVERPROPERTY('ResourceLastUpdateDateTime') AS [LastUpdateDateTime],SERVERPROPERTY('ComparisonStyle') AS [ComparisonStyle],
	SERVERPROPERTY('LCID') AS [LCID],SERVERPROPERTY('SqlCharSet') AS [SqlCharSet],SERVERPROPERTY('SqlCharSetName') AS [SqlCharSetName], SERVERPROPERTY('SqlSortOrder') AS [SqlSortOrder]";
		/// <summary>
		/// 数据库镜像端点既支持数据库镜像伙伴之间的会话，也支持 Always On 可用性组的主副本与其辅助副本之间的见证服务器和会话。
		/// </summary>
		public static string _sqlstrDB_Mirroring_Endpoints = @"set nocount on;
	SELECT   
		@@SERVERNAME as ServerName, 
		name=cast(name as varchar(30)),
		endpoint_id, principal_id, 
		protocol_desc=cast(protocol_desc as varchar(20)),
		type_desc=cast(type_desc as varchar(30)),
		state_desc=cast(state_desc as varchar(20)),
		is_admin_endpoint,
		role_desc=cast(role_desc as varchar(30)),
		is_encryption_enabled,
		connection_auth_desc=cast(connection_auth_desc as varchar(30)),
		encryption_algorithm_desc=cast(encryption_algorithm_desc as varchar(20))
	from sys.database_mirroring_endpoints";
		/// <summary>
		/// AG_Cluster_Info 故障转移群集信息
		/// /// 
		/// </summary>
		public static string _sqlstrAG_Cluster_Info = @"set nocount on;
	select  @@SERVERNAME as ServerName, cluster_name=cast(c.cluster_name as char(30)), 
	quorum_type=cast(c.quorum_type_desc as char(30)), 
	quorum_state_desc=cast(c.quorum_state_desc as char(30))
	from sys.dm_hadr_cluster c";
		/// <summary>
		/// 故障转移群集诊断日志配置
		/// </summary>
		public static string _sqlstrserver_diagnostics_log = @"select @@SERVERNAME as ServerName, * from sys.dm_os_server_diagnostics_log_configurations";

		/// <summary>
		/// ///AlwasyON数据库状态配置
		/// </summary>
		public static string _sqlstrAG_DB_State_Config = @"set nocount on; 
	select @@SERVERNAME as ServerName, 
	database_name=cast(drcs.database_name as varchar(30)), 
	drs.database_id,
	drs.group_id,
	drs.replica_id,
	drs.is_local,
	drcs.is_failover_ready,
	drcs.is_pending_secondary_suspend,
	drcs.is_database_joined,
	drs.is_suspended,
	drs.is_commit_participant,
	suspend_reason_desc=cast(drs.suspend_reason_desc as varchar(30)),
	synchronization_state_desc=cast(drs.synchronization_state_desc as varchar(30)),
	synchronization_health_desc=cast(drs.synchronization_health_desc as varchar(30)),
	database_state_desc=cast(drs.database_state_desc as varchar(30)),
	drs.last_sent_lsn,
	drs.last_sent_time,
	drs.last_received_lsn,
	drs.last_received_time,
	drs.last_hardened_lsn,
	drs.last_hardened_time,
	drs.last_redone_lsn,
	drs.last_redone_time,
	drs.log_send_queue_size,
	drs.log_send_rate,
	drs.redo_queue_size,
	drs.redo_rate,
	drs.filestream_send_rate,
	drs.end_of_log_lsn,
	drs.last_commit_lsn,
	drs.last_commit_time,
	drs.low_water_mark_for_ghosts,
	drs.recovery_lsn,
	drs.truncation_lsn,
	pr.file_id,
	pr.error_type,
	pr.page_id,
	pr.page_status,
	pr.modification_time
	from sys.dm_hadr_database_replica_cluster_states drcs join 
	sys.dm_hadr_database_replica_states drs on drcs.replica_id=drs.replica_id
	and drcs.group_database_id=drs.group_database_id left outer join
	sys.dm_hadr_auto_page_repair pr on drs.database_id=pr.database_id 
	order by drs.database_id";

		////AlwasyON状态识别
		public static string _sqlstrAG_State_Identification = @"SELECT 
	group_name=cast(arc.group_name as varchar(30)), 
	replica_server_name=cast(arc.replica_server_name as varchar(30)), 
	node_name=cast(arc.node_name as varchar(30)),
	ars.is_local, 
	role_desc=cast(ars.role_desc as varchar(30)), 
	availability_mode=cast(ar.availability_mode as varchar(30)),
	ar.availability_mode_Desc,
	failover_mode_desc=cast(ar.failover_mode_desc as varchar(30)),
	join_state_desc=cast(arcs.join_state_desc as varchar(30)),
	operational_state_desc=cast(ars.operational_state_desc as varchar(30)), 
	connected_state_desc=cast(ars.connected_state_desc as varchar(30)), 
	recovery_health_desc=cast(ars.recovery_health_desc as varchar(30)), 
	synhcronization_health_desc=cast(ars.synchronization_health_desc as varchar(30)),
	ars.last_connect_error_number, 
	last_connect_error_description=cast(ars.last_connect_error_description as varchar(30)), 
	ars.last_connect_error_timestamp,
	endpoint_url=cast (ar.endpoint_url as varchar(30)),
	ar.session_timeout,
	primary_role_allow_connections_desc=cast(ar.primary_role_allow_connections_desc as varchar(30)),
	secondary_role_allow_connections_desc=cast(ar.secondary_role_allow_connections_desc as varchar(30)),
	ar.create_date,
	ar.modify_date,
	ar.backup_priority, 
	ar.read_only_routing_url,
	arcs.replica_id, 
	arcs.group_id
from sys.dm_hadr_availability_replica_cluster_nodes arc 
join sys.dm_hadr_availability_replica_cluster_states arcs on arc.replica_server_name=arcs.replica_server_name
join sys.dm_hadr_availability_replica_states ars on arcs.replica_id=ars.replica_id
join sys.availability_replicas ar on ars.replica_id=ar.replica_id
join sys.availability_groups ag 
on ag.group_id = arcs.group_id 
and ag.name = arc.group_name 
--dm_hadr_availability_replica_cluster_nodes doesn't have a group_id, so we have to join by name
ORDER BY 
cast(arc.group_name as varchar(30)), 
cast(ars.role_desc as varchar(30))";

	//	AlwasyON状态配置
		public static string _sqlstrAG_State_Config = @"SELECT      @@SERVERNAME as ServerName,
		availability_group=cast(ag.name as varchar(30)), 
	primary_replica=cast(ags.primary_replica as varchar(30)),
	primary_recovery_health_desc=cast(ags.primary_recovery_health_desc as varchar(30)),
	synchronization_health_desc=cast(ags.synchronization_health_desc as varchar(30)),
	ag.group_id, ag.resource_id, ag.resource_group_id, ag.failure_condition_level, 
	ag.health_check_timeout, 
	automated_backup_preference_desc=cast(ag.automated_backup_preference_desc as varchar(10))
	from sys.availability_groups ag join sys.dm_hadr_availability_group_states ags
	on ag.group_id=ags.group_id";

		//	群集仲裁网络
		public static string _sqlstrWC_Quorum_Network = @"SELECT      @@SERVERNAME as ServerName, 
	member_name=cast(cm.member_name as varchar(30)), 
	member_type_desc=cast(cm.member_type_desc as varchar(30)), 
	member_state_desc=cast(cm.member_state_desc as varchar(10)),
	cm.number_of_quorum_votes,
	cast(cn.network_subnet_ip as varchar(40)),
	cast(cn.network_subnet_ipv4_mask as varchar(40)),
	cn.network_subnet_prefix_length,
	cn.is_public,
	cn.is_ipv4
	from sys.dm_hadr_cluster_members cm join sys.dm_hadr_cluster_networks cn
	on cm.member_name=cn.member_name";
		//数据库镜像权限
		public static string _sqlstrDB_Mirroring_Permission = @"set nocount on;
	SELECT      @@SERVERNAME as ServerName,
		cast(perm.class_desc as varchar(30)) as [ClassDesc], cast(prin.name as varchar(30)) [Principal],
		cast(perm.permission_name as varchar(30)) as [Permission],
		cast(perm.state_desc as varchar(30)) as [StateDesc],
		cast(prin.type_desc as varchar(30)) as [PrincipalType],
		prin.is_disabled 
	FROM sys.server_permissions perm
	LEFT JOIN sys.server_principals prin ON perm.grantee_principal_id = prin.principal_id
	LEFT JOIN sys.tcp_endpoints tep  ON perm.major_id = tep.endpoint_id
	WHERE perm.class_desc = 'ENDPOINT' AND perm.permission_name = 'CONNECT' AND tep.type = 4";


		//工作进程数
		public static string _sqlstrWorker_Count = @"set nocount on;
		SELECT      @@SERVERNAME as ServerName, 
				 db_name() as databasename, 
				 CONVERT (varchar(30), getdate(), 121) as runtime, 
					 sum(current_tasks_count) current_tasks_count,
				 sum(current_workers_count) current_workers_count,
				 sum(active_workers_count) active_workers_count,
				 sum(work_queue_count) work_queue_count
		FROM sys.dm_os_schedulers
		WHERE STATUS = 'Visible Online'
		option(recompile);";

		//内存状态
		public static string _sqlstrMemory_Pressure = @" select @@SERVERNAME AS ServerName,physical_memory_in_use_kb/(1024) as sql_physmem_inuse_mb,
			locked_page_allocations_kb/(1024) as awe_memory_mb,
			total_virtual_address_space_kb/(1024) as max_vas_mb,
			virtual_address_space_committed_kb/(1024) as sql_committed_mb,
			memory_utilization_percentage as working_set_percentage,
			virtual_address_space_available_kb/(1024) as vas_available_mb,
			process_physical_memory_low as is_there_external_pressure,
			process_virtual_memory_low as is_there_vas_pressure
			from sys.dm_os_process_memory";

		//内存索引统计
		public static string _sqlstrIN_MEMORY_Index_Stats = @"
				if OBJECT_ID('tempdb..#tmp_databases') is not null
					drop table #tmp_databases;
				if OBJECT_ID('tempdb..#Ind') is not null
					drop table #Ind; 

				CREATE TAble #tmp_databases
				(
				id int identity(1,1),
				dbname varchar(200)
				);
				CREATE TABLE #Ind
				(
					dbname varchar(200),
					IndexFullName	nvarchar(300),
					page_update_count_pct	float,
					page_consolidation_count_pct	float,
					page_split_count_pct	float,
					key_split_count_pct	float,
					page_merge_count_pct	float,
					key_merge_count_pct	float
				)

				insert into #tmp_databases
				select name from sys.databases where is_read_only=0 and state_desc='ONLINE' --and name not in ('master','model','tempdb','msdb');

				DECLARE @min int=1,@max int =0
				select @max=count(*) from #tmp_databases

				while(@min <=@max)
				begin
				DECLARE @SQL VARCHAR(8000) = ''
				,@dbname varchar(200) = ''

				SELECT @dbname=dbname from #tmp_databases where id = @min

				SELECT @SQL=' USE ['+@dbname+']	
	
					INSERT INTO #Ind
					SELECT DISTINCT DB_NAME() AS DBNAME,CONCAT(t.name,''.'',o.name,''.'',si.name) AS IndexFullName,FLOOR((CAST(page_update_retry_count AS FLOAT)/CASE WHEN page_update_count=0 THEN 1 ELSE page_update_count END)*100) AS page_update_count_pct,FLOOR((CAST(page_consolidation_retry_count AS FLOAT)/CASE WHEN page_consolidation_count=0 THEN 1 ELSE page_consolidation_count END)*100) AS page_consolidation_count_pct,FLOOR((CAST(page_split_retry_count AS FLOAT)/CASE WHEN page_split_count=0 THEN 1 ELSE page_split_count END)*100) AS page_split_count_pct,FLOOR((CAST(key_split_retry_count AS FLOAT)/CASE WHEN key_split_count=0 THEN 1 ELSE key_split_count END)*100) AS key_split_count_pct,FLOOR((CAST(page_merge_retry_count AS FLOAT)/CASE WHEN page_merge_count=0 THEN 1 ELSE page_merge_count END)*100) AS page_merge_count_pct,FLOOR((CAST(key_merge_retry_count AS FLOAT)/CASE WHEN key_merge_count=0 THEN 1 ELSE key_merge_count END)*100) AS key_merge_count_pct 
					FROM sys.dm_db_xtp_nonclustered_index_stats AS xnis (NOLOCK) 
					INNER JOIN sys.dm_db_xtp_index_stats AS xis (NOLOCK) ON xis.[object_id]=xnis.[object_id] AND xis.[index_id]=xnis.[index_id] INNER JOIN sys.indexes AS si (NOLOCK) ON xis.[object_id]=si.[object_id] AND xis.[index_id]=si.[index_id] INNER JOIN sys.objects AS o (NOLOCK) ON si.[object_id]=o.[object_id] INNER JOIN sys.tables AS mst (NOLOCK) ON mst.[object_id]=o.[object_id] INNER JOIN sys.schemas AS t (NOLOCK) ON t.[schema_id]=mst.[schema_id] 
					WHERE o.[type]=''U''

					'
					PRINT @SQL
						EXEC (@SQL) 
						set @min = @min+1
					end		
					select @@SERVERNAME as ServerName,* from #Ind
					DROP TABLE #Ind
					DROP TABLE #tmp_databases ";

		//内存索引统计
		public static string _sqlstrColumnStore_Index_Stats = @"if OBJECT_ID('tempdb..#tmp_databases') is not null
							drop table #tmp_databases;
						if OBJECT_ID('tempdb..#Ind') is not null
							drop table #Ind; 

						CREATE TAble #tmp_databases
						(
						id int identity(1,1),
						dbname varchar(200)
						);
						CREATE TABLE #Ind
						(
							dbname varchar(200),
							schema_name	nvarchar(300),
							object_name	nvarchar(300),
							index_name	nvarchar(300),
							index_type	nvarchar(300),
							avg_fragmentation_in_percent	float 
						)

						insert into #tmp_databases
						select name from sys.databases where is_read_only=0 and state_desc='ONLINE' --and name not in ('master','model','tempdb','msdb');

						DECLARE @min int=1,@max int =0
						select @max=count(*) from #tmp_databases

						while(@min <=@max)
						begin
						DECLARE @SQL VARCHAR(8000) = ''
						,@dbname varchar(200) = ''

						SELECT @dbname=dbname from #tmp_databases where id = @min

						SELECT @SQL=' USE ['+@dbname+']	
		
							INSERT INTO #Ind
							SELECT DB_NAME() AS DBNAME,OBJECT_SCHEMA_NAME(i.object_id) AS schema_name,
									OBJECT_NAME(i.object_id) AS object_name,
									i.name AS index_name,
									i.type_desc AS index_type,
									100.0 * (ISNULL(SUM(rgs.deleted_rows), 0)) / NULLIF(SUM(rgs.total_rows), 0) AS avg_fragmentation_in_percent
							FROM sys.indexes AS i
							INNER JOIN sys.dm_db_column_store_row_group_physical_stats AS rgs ON i.object_id = rgs.object_id  AND i.index_id = rgs.index_id
							WHERE rgs.state_desc = ''COMPRESSED''
							GROUP BY i.object_id, i.index_id, i.name, i.type_desc
							ORDER BY schema_name, object_name, index_name, index_type;

							'
							--PRINT @SQL
								EXEC (@SQL) 
								set @min = @min+1
							end		
							select @@SERVERNAME as ServerName,* from #Ind
							DROP TABLE #Ind
							DROP TABLE #tmp_databases ";

		/// 数据库故障转移群集
		public static string _sqlstrDatabaseHADR = @"
					declare @version char(12); 
					set @version =  convert(char(12),serverproperty('productversion')); 
					declare @sqlstatment nvarchar(4000) 
					set @sqlstatment = 'SELECT @@SERVERNAME ServerName, dbs.name AS 
					DatabaseName, CASE  WHEN (mirroring_guid is null) THEN ''False'' ELSE ''True'' END as ''IsMirrored'',
					CASE WHEN EXISTS(select 1 FROM msdb.dbo.log_shipping_primary_databases lsdb where lsdb.primary_database=dbs.name ) OR EXISTS(select 1 FROM msdb.dbo.log_shipping_secondary_databases lsdb where lsdb.secondary_database=dbs.name)
					THEN ''True'' ELSE ''False'' END IsLogShippingEnabled,' 
					if (select CAST(substring(@version, 1, 2) AS int))>=11 
					begin set @sqlstatment = @sqlstatment + 'CASE WHEN is_published=0 THEN ''False'' ELSE ''True'' END IsPublished,CASE WHEN is_subscribed=0 THEN ''False'' ELSE ''True'' END IsSubscribed,CASE WHEN is_merge_published=0 THEN ''False'' ELSE ''True'' END IsMergePublished,CASE WHEN is_distributor=0 THEN ''False'' ELSE ''True'' END IsDistributor,CASE WHEN db_replica_state.group_id IS NULL THEN ''False'' ELSE ''True'' END IsMemberOfAG, ag.name as 
					''AvailabilityGroup'', db_replica_state.synchronization_state_desc SynchronizationState, agl.dns_name DnsName,replica_states.role_desc ReplicaRole, replicas.secondary_role_allow_connections_desc SecondaryRoleAllowConnections FROM master.sys.databases dbs WITH (NOLOCK) LEFT OUTER JOIN 
					sys.database_mirroring dbm ON dbs.database_id = dbm.database_id LEFT OUTER JOIN sys.dm_hadr_database_replica_states AS db_replica_state ON dbs.database_id = 
					db_replica_state.database_id and db_replica_state.is_local=1 LEFT OUTER JOIN sys.availability_replicas AS replicas ON db_replica_state.replica_id = replicas.replica_id and replicas.replica_server_name = SERVERPROPERTY(''servername'') LEFT OUTER JOIN sys.dm_hadr_availability_replica_states replica_states ON replica_states.replica_id = replicas.replica_id and replica_states.group_id=replicas.group_id and replica_states.is_local =1 LEFT OUTER JOIN sys.availability_groups AS ag ON ag.group_id = db_replica_state.group_id LEFT OUTER JOIN sys.availability_group_listeners AS agl ON ag.group_id = agl.group_id ORDER BY DatabaseName' end 
					else if  '10' = (select substring(@version, 1, 2)) or  '9' = (select substring(@version, 1, 1)) 
					begin	 
					set @sqlstatment=@sqlstatment +	 'CASE WHEN is_published=0 THEN ''False'' ELSE ''True'' END IsPublished,CASE WHEN is_subscribed=0 THEN ''False'' ELSE ''True'' END IsSubscribed,CASE WHEN is_merge_published=0 THEN ''False'' ELSE ''True'' END IsMergePublished,CASE WHEN is_distributor=0 THEN ''False'' ELSE ''True'' END IsDistributor,''n/a'' IsMemberOfAG, ''n/a'' as AvailabilityGroup, ''n/a'' SynchronizationState, ''n/a'' as DnsName,  ''n/a'' as ReplicaRole, ''n/a'' as SecondaryRoleAllowConnections FROM master.sys.databases dbs WITH (NOLOCK) LEFT OUTER JOIN sys.database_mirroring dbm ON dbs.database_id = dbm.database_id   ORDER BY DatabaseName' 
					end 
					exec sp_executesql @sqlstatment";

		/// AG监听IP
		public static string _sqlstrAG_Listener_IP = @"select @@SERVERNAME ServerName,l.listener_id,
					state_desc=cast(lia.state_desc as varchar(20)),
					dns_name=cast(l.dns_name as varchar(30)),
					 l.port, l.is_conformant,
					ip_configuration_string_from_cluster=cast(l.ip_configuration_string_from_cluster as varchar(40)),
					ip_address=cast(lia.ip_address as varchar(30)),
					lia.ip_subnet_mask, lia.is_dhcp, 
					network_subnet_ip=cast(lia.network_subnet_ip as varchar(30)),
					lia.network_subnet_prefix_length,
					network_subnet_ipv4_mask=cast(lia.network_subnet_ipv4_mask as varchar(30)),
					lia.network_subnet_prefix_length
				from sys.availability_group_listeners l 
				join sys.availability_group_listener_ip_addresses lia on l.listener_id=lia.listener_id
				option(recompile);";

		/// 路由信息
		public static string _sqlstrRouting_list_Info = @"SELECT @@SERVERNAME ServerName,cast(ar.replica_server_name as varchar(30)) [When This Server is Primary], 
				rl.routing_priority [Priority], 
				cast(ar2.replica_server_name as varchar(30)) [Route to this Server],
				ar.secondary_role_allow_connections_desc [Connections Allowed],
				cast(ar2.read_only_routing_url as varchar(50)) as [Routing URL]
				FROM sys.availability_read_only_routing_lists rl
				  inner join sys.availability_replicas ar on rl.replica_id = ar.replica_id
				  inner join sys.availability_replicas ar2 on rl.read_only_replica_id = ar2.replica_id
				ORDER BY ar.replica_server_name, rl.routing_priority";

		/// <summary>
		/// 数据库大小
		/// </summary>
		public static string _sqlstrDatabaseSize = @"
					DECLARE @Version int, @Command NVARCHAR(max)
					SET @Version=CONVERT (int,REPLACE (LEFT (CONVERT (varchar, SERVERPROPERTY ('ProductVersion')),2), '.', ''))
					IF @Version > 10
					BEGIN
						SET @Command='create table #Test (InstanceName sysname, DatabaseName sysname, DBSize_MB decimal(20,2),TotalDataSize_MB decimal(20,2),DataAllocatedSize_MB decimal(20,2), [DataUsedSize_MB] decimal(20,2),[TotalLogSize_MB] decimal(20,2),[LogUsedSize_MB] decimal(20,2))
							declare @SQL nvarchar(max)
							select @SQL = coalesce(@SQL,'''') + 
							''USE '' + QUOTENAME(Name) + ''
							insert into #Test
							SELECT @@SERVERNAME ServerName, DB_NAME() DatabaseName,(SELECT SUM(CAST(df.size as float)) FROM sys.database_files AS df ) /128 AS [DBSize_MB],(SELECT SUM(CAST(df.size as float)) FROM sys.database_files AS df ) /128 - (SELECT SUM(CAST(df.size as float)) FROM sys.database_files AS df WHERE df.type in (1, 3))/128 AS TotalDataSize,SUM(a.total_pages)/128 AS [DataAllocatedSize_MB],SUM( a.used_pages)/128 AS [DataUsedSize_MB],(SELECT SUM(CAST(df.size as float)) FROM sys.database_files AS df WHERE df.type in (1, 3))/128 AS [TotalLogSize_MB],
							(select SUM(CAST(FILEPROPERTY(df.name, ''''SpaceUsed'''') AS INT)) from sys.database_files df WHERE df.type in (1, 3))/128 as [LogUsedSize_MB] 
							FROM sys.allocation_units a; '' from sys.databases where state_desc=''ONLINE'' and (replica_id IS NULL OR replica_id NOT IN (SELECT replica_id FROM sys.dm_hadr_availability_replica_states ars (NOLOCK) WHERE ars.role NOT IN (1)))
							execute(@SQL)
							select InstanceName, DatabaseName,DBSize_MB,TotalDataSize_MB,DataAllocatedSize_MB,DataUsedSize_MB,TotalLogSize_MB,LogUsedSize_MB from #Test order by DatabaseName 
							drop table #Test
							' 
						EXECUTE sp_executesql @Command
					END	
					ELSE
					BEGIN
						SET @Command='create table #Test (InstanceName sysname, DatabaseName sysname, DBSize_MB decimal(20,2),TotalDataSize_MB decimal(20,2),DataAllocatedSize_MB decimal(20,2), [DataUsedSize_MB] decimal(20,2),[TotalLogSize_MB] decimal(20,2),[LogUsedSize_MB] decimal(20,2))
							declare @SQL nvarchar(max)
							select @SQL = coalesce(@SQL,'''') + 
							''USE '' + QUOTENAME(Name) + ''
							insert into #Test
							SELECT @@SERVERNAME ServerName, DB_NAME() DatabaseName,(SELECT SUM(CAST(df.size as float)) FROM sys.database_files AS df ) /128 AS [DBSize_MB],(SELECT SUM(CAST(df.size as float)) FROM sys.database_files AS df ) /128 - (SELECT SUM(CAST(df.size as float)) FROM sys.database_files AS df WHERE df.type in (1, 3))/128 AS TotalDataSize,SUM(a.total_pages)/128 AS [DataAllocatedSize_MB],SUM( a.used_pages)/128 AS [DataUsedSize_MB],(SELECT SUM(CAST(df.size as float)) FROM sys.database_files AS df WHERE df.type in (1, 3))/128 AS [TotalLogSize_MB],
							 (select SUM(CAST(FILEPROPERTY(df.name, ''''SpaceUsed'''') AS INT)) from sys.database_files df WHERE df.type in (1, 3))/128 as [LogUsedSize_MB] 
							FROM sys.allocation_units a; '' from sys.databases where state_desc=''ONLINE'' 
							execute(@SQL)
							select InstanceName, DatabaseName,DBSize_MB,TotalDataSize_MB,DataAllocatedSize_MB,DataUsedSize_MB,TotalLogSize_MB,LogUsedSize_MB from #Test order by DatabaseName 
							drop table #Test
							'
						  EXECUTE sp_executesql @Command
					END";
		///复制错误
		public static string _sqlstrReplicationErrors = @"if exists(select 1 from sys.databases where name like '%distribution%')
				begin
					select top 500 *
					from [distribution].[dbo].MSrepl_errors
					where DATEDIFF(day,getdate(),MSrepl_errors.[time]) < 5
					order by MSrepl_errors.[time] desc 
				end";

		////服务状态

		public static string _sqlstrSQLSvc_Status = @"select 
				@@SERVERNAME as ServerName,
				servicename, 
				status_desc, 
				RTRIM(CONVERT(CHAR(17),DATEDIFF(second,convert(datetime,last_startup_time),getdate())/86400)) + ':' + 
				RIGHT('00'+RTRIM(CONVERT(CHAR(7),DATEDIFF(second,convert(datetime,last_startup_time),getdate())%86400/3600)),2) + ':' + 
				RIGHT('00'+RTRIM(CONVERT(CHAR(7),DATEDIFF(second,convert(datetime,last_startup_time),getdate())%86400%3600/60)),2) + ':' + 
				RIGHT('00'+RTRIM(CONVERT(CHAR(7),DATEDIFF(second,convert(datetime,last_startup_time),getdate())%86400%3600%60)),2) AS [Service_Uptime D:H:M:S]
			from sys.dm_server_services ";
		/// <summary>
		/// 登录信息
		/// </summary>
		public static string _sqlstrLogin_Info = @"SELECT @@SERVERNAME ServerName, SQLLogins = (SELECT COUNT(*) FROM sys.server_principals WHERE type_desc = 'SQL_LOGIN' and name not like '##%##'),
				WindowsLogins = (SELECT COUNT(*) FROM sys.server_principals WHERE type_desc='WINDOWS_LOGIN' and name not like 'NT SERVICE\%' and name not like 'NT AUTHORITY\%'),
				WindowsGroups = (SELECT COUNT(*) FROM sys.server_principals WHERE type_desc = 'WINDOWS_GROUP'),
				Credentials=(SELECT COUNT(*) FROM sys.credentials WHERE name not like '##%##'),
				Proxies=(SELECT COUNT(*) FROM msdb..sysproxies)";

		/// <summary>
		/// /资源池信息
		/// </summary>
		public static string _sqlstrResourceGroupPools = @"DECLARE @Version int, @Command NVARCHAR(1000)
					SET @Version=convert (int,REPLACE (LEFT (CONVERT (varchar, SERVERPROPERTY ('ProductVersion')),2), '.', ''))
					IF @Version > 9
					BEGIN
						SET @Command='Select  @@SERVERNAME ServerName, name PoolName,min_cpu_percent MinCPUPct,max_cpu_percent MaxCPUPct,min_memory_percent MinMemoryPct,max_memory_percent MaxMemoryPct from sys.resource_governor_resource_pools' 
						EXEC sp_executesql @Command
					END";
		/// <summary>
		/// 端点信息
		/// </summary>
		public static string _sqlstrEndpoints = @"Select  @@SERVERNAME ServerName, e.name EndpointName, e.protocol_desc ProtocolDesc, e.type_desc Type, e.state_desc State 
							from sys.endpoints e 
							join sys.server_principals sp on e.principal_id = sp.principal_id";


		/// <summary>
		/// 服务器触发器
		/// </summary>

		public static string _sqlstrServerTriggers = @"Select @@SERVERNAME ServerName,  name TriggerName, type_desc TriggerType, parent_class_desc TriggerLevel, create_date CreationDate,modify_date ModifyDate,is_disabled IsDisabled, is_ms_shipped IsMSShipped 
							from master.sys.server_triggers";

		/// <summary>
		/// 工作负荷组
		/// </summary>
		public static string _sqlstrResourceGovernorGroups = @"DECLARE @Version int, @Command NVARCHAR(1000)
						SET @Version=convert (int,REPLACE (LEFT (CONVERT (varchar, SERVERPROPERTY ('ProductVersion')),2), '.', ''))
						IF @Version > 9
						BEGIN
							SET @Command='Select @@SERVERNAME ServerName,  name GroupName, importance Importance, request_max_memory_grant_percent RequestMaxMemoryGrantPct, request_max_cpu_time_sec RequestMaxCPUTimeSec, request_memory_grant_timeout_sec RequestMemoryGrantTimeoutSec, max_dop MaxDop, group_max_requests GroupMaxRequests from sys.resource_governor_workload_groups' 
							EXEC sp_executesql @Command
						END";
		/// <summary>
		/// 服务器审计
		/// </summary>
		public static string _sqlstrServerAudits = @"DECLARE @Version int, @Command NVARCHAR(1000)
				SET @Version=convert (int,REPLACE (LEFT (CONVERT (varchar, SERVERPROPERTY ('ProductVersion')),2), '.', ''))
				IF @Version > 9
				BEGIN
					SET @Command='SELECT @@SERVERNAME ServerName,  sa.name AuditName,sa.type_desc AuditDestination,sa.on_failure_desc OnFailure, sa.queue_delay QueueDelay, sa.is_state_enabled IsEnabled,sa.create_date CreationDate,sa.modify_date ModificationDate,log_file_path AuditPath, max_rollover_files MaxRollerFiles, max_file_size MaxFileSize,reserve_disk_space ReserveDiskSpace from sys.server_audits sa left join sys.server_file_audits sfa on sa.audit_id = sfa.audit_id' 
					EXEC sp_executesql @Command
				END";
		/// <summary>
		/// 服务器审计规则
		/// </summary>
		public static string _sqlstrServerAuditSpecifications = @"DECLARE @Version int, @Command NVARCHAR(1000)
				SET @Version=convert (int,REPLACE (LEFT (CONVERT (varchar, SERVERPROPERTY ('ProductVersion')),2), '.', ''))
				IF @Version > 9
				BEGIN
					SET @Command='select @@SERVERNAME ServerName,  sas.name AuditSpecificationName, sas.create_date CreationDate,sas.modify_date ModifyDate, sas.is_state_enabled IsEnabled, sasd.audit_action_name AuditAction from sys.server_audit_specifications sas join sys.server_audit_specification_details sasd on sas.server_specification_id=sasd.server_specification_id' 
					EXEC sp_executesql @Command
				END";

		/// <summary>
		/// 备份设备
		/// </summary>
		public static string _sqlstrBackupDevices = @"Select @@SERVERNAME ServerName,  name BackupDevice, type DeviceType, physical_name PhysicalName  
							from sys.backup_devices";

		/// <summary>
		/// /链接服务器 
		/// </summary>
		public static string _sqlstrLinkedServers = @"select @@SERVERNAME ServerName, server_id ServerId, name ServerName, product Product, provider Provider, data_source DataSource, location Location,provider_string ProviderString, catalog Catalog, connect_timeout ConnectTimeout, query_timeout QueryTimeout, is_linked IsLinked, is_remote_login_enabled IsRemoteLoginEnabled, is_rpc_out_enabled IsRpcOutEnabled, is_data_access_enabled IsDataAccessEnabled,is_collation_compatible IsCollationCompatible,uses_remote_collation UseRemoteCollation,collation_name Collation,lazy_schema_validation LazySchemaValidation,is_system IsSystem,is_publisher IsPublisher,is_subscriber IsSubscriber,is_distributor IsDistributor,is_nonsql_subscriber IsNonsqlSubscriber, modify_date ModifyDate 
				from sys.servers";

		/// <summary>
		/// 策略管理
		/// </summary>
		public static string _sqlstrPolicyBasedManagement = @"DECLARE @Version int, @Command NVARCHAR(1000)
						SET @Version=convert (int,REPLACE (LEFT (CONVERT (varchar, SERVERPROPERTY ('ProductVersion')),2), '.', ''))
						IF @Version > 9
						BEGIN
							SET @Command='SELECT @@SERVERNAME ServerName, p.name PolicyName,p.is_enabled IsEnabled, CASE p.execution_mode WHEN 0 THEN ''On demand'' WHEN 1 THEN ''On change: prevent'' WHEN 2 THEN ''On change: log only'' WHEN 4 THEN ''On schedule'' END ExecutionMode,c.name ConditionName, c.facet Facet, p.date_created DateCreated,p.date_modified DateModified FROM msdb.dbo.syspolicy_policies p INNER JOIN msdb.dbo.syspolicy_conditions c ON p.condition_id = c.condition_id ORDER  BY p.name' 
							EXEC sp_executesql @Command
						END";
		/// <summary>
		/// //性能数据收集
		/// </summary>
		public static string _sqlstrPerfDataCollection = @"DECLARE @Version int, @Command NVARCHAR(1000)
								SET @Version = convert(int, REPLACE(LEFT(CONVERT(varchar, SERVERPROPERTY('ProductVersion')), 2), '.', ''))
						IF @Version > 9
						BEGIN
							SET @Command='select @@SERVERNAME ServerName, collection_set_id CollectionSetId,a.name CollectionName,is_running IsRunning, case collection_mode when 1 then ''non-cached'' when 0 then ''cached'' end  CollectionMode, days_until_expiration DaysUntilExpiration,b.name ScheduleName from msdb.dbo.syscollector_collection_sets a,msdb.dbo.sysschedules b where a.schedule_uid=b.schedule_uid' 
							EXEC sp_executesql @Command
						END";


		/// <summary>
		/// 扩展事件
		/// </summary>
		public static string _sqlstrExtendedEvents = @"DECLARE @Version int, @Command NVARCHAR(1000)
						SET @Version=convert (int,REPLACE (LEFT (CONVERT (varchar, SERVERPROPERTY ('ProductVersion')),2), '.', ''))
						IF @Version > 9
						BEGIN
							SET @Command='select @@SERVERNAME ServerName, event_session_id EventSessionId, name, event_retention_mode_desc RetentionMode, max_dispatch_latency MaxDispatchLatency, max_memory MaxMemory, max_event_size MaxEventSize, track_causality TrackCausality, startup_state StartupState from sys.server_event_sessions' 
							EXEC sp_executesql @Command
						END
						";
		/// <summary>
		/// 数据库邮件
		/// </summary>
		public static string _sqlstrDatabaseMail = @"select @@SERVERNAME ServerName, p.profile_id ProfileId, p.name ProfileName, a.last_mod_datetime AccountLastModified,p.last_mod_datetime ProfileLastModified from msdb.dbo.sysmail_profileaccount pa join msdb.dbo.sysmail_profile p on pa.profile_id = p.profile_id join msdb.dbo.sysmail_account a on pa.account_id = a.account_id order by p.profile_id, a.account_id";

		/// <summary>
		/// SQLTraces配置
		/// </summary>
		public static string _sqlstrSqlTraces = @"select @@SERVERNAME ServerName, id TraceId, path Path, CASE status WHEN 0 THEN 'Stopped' ELSE 'Started' END TraceStatus, max_size MaxSize, start_time StartTime, stop_time StopTime, last_event_time LastEventTime from sys.traces";


		/// <summary>
		/// 代理jobs
		/// </summary>
		 public static string _sqlstrAgentJobs = @"SELECT @@SERVERNAME ServerName, [sJOB].[name] [JobName], [sCAT].[name] [JobCategory], [sJOB].[description] [JobDescription], CASE [sJOB].[enabled] WHEN 1 THEN 'Yes' WHEN 0 THEN 'No' END [IsEnabled], [sJOB].[date_created] [JobCreatedOn], [sJOB].[date_modified] [JobLastModifiedOn] , [sSVR].[name] [OriginatingServerName] , [sJSTP].[step_name] [JobStartStepName] , CASE WHEN [sSCH].[schedule_uid] IS NULL THEN 'No' ELSE 'Yes'   END [IsScheduled] , [sSCH].[name] [JobScheduleName] , CASE [sJOB].[delete_level] WHEN 0 THEN 'Never' WHEN 1 THEN 'On Success' WHEN 2 THEN 'On Failure' WHEN 3 THEN 'On Completion' END [JobDeletionCriterion] FROM [msdb].[dbo].[sysjobs] [sJOB] LEFT JOIN [msdb].[sys].[servers] [sSVR] ON [sJOB].[originating_server_id] = [sSVR].[server_id] LEFT JOIN [msdb].[dbo].[syscategories] [sCAT] ON [sJOB].[category_id] = [sCAT].[category_id] LEFT JOIN [msdb].[dbo].[sysjobsteps] [sJSTP] ON [sJOB].[job_id] = [sJSTP].[job_id] AND [sJOB].[start_step_id] = [sJSTP].[step_id] LEFT JOIN [msdb].[sys].[syslogins] [sDBP] ON [sJOB].[owner_sid] = [sDBP].[sid] LEFT JOIN [msdb].[dbo].[sysjobschedules] [sJOBSCH] ON [sJOB].[job_id] = [sJOBSCH].[job_id] LEFT JOIN [msdb].[dbo].[sysschedules] [sSCH] ON [sJOBSCH].[schedule_id] = [sSCH].[schedule_id]";

		/// <summary>
		/// 执行历史
		/// </summary>
		 public static string _sqlstrAgentJob_Run_History = @"	SELECT 
						@@SERVERNAME AS ServerName
						,j.[name], jh.run_status,
						MAX(CAST( 
						STUFF(STUFF(CAST(jh.run_date as varchar),7,0,'-'),5,0,'-') + ' ' + 
						STUFF(STUFF(REPLACE(STR(jh.run_time,6,0),' ','0'),5,0,':'),3,0,':') as datetime)) AS [LastRun], 
						CASE jh.run_status WHEN 0 THEN 'Failed' WHEN 1 THEN 'Success' WHEN 2 THEN 'Retry' WHEN 3 THEN 'Canceled' WHEN 4 THEN 'In progress'  END AS [Status]
						FROM msdb.dbo.sysjobs j 
						INNER JOIN msdb.dbo.sysjobhistory jh ON jh.job_id = j.job_id AND jh.step_id = 0 
						inner join msdb.dbo.syscategories sc on j.category_id = sc.category_id 
						GROUP BY j.[name], jh.run_status 
						HAVING MAX(CAST( 
						STUFF(STUFF(CAST(jh.run_date as varchar),7,0,'-'),5,0,'-') + ' ' + 
						STUFF(STUFF(REPLACE(STR(jh.run_time,6,0),' ','0'),5,0,':'),3,0,':') as datetime)) >= DATEADD(DAY, -7, GETDATE());";

		/// <summary>
		/// job步骤
		/// </summary>
		 public static string _sqlstrAgentJobSteps = @"SELECT @@SERVERNAME ServerName, [sJOB].[name] [JobName], [sJSTP].[step_id] [StepNo], [sJSTP].[step_name] [StepName], CASE [sJSTP].[subsystem] WHEN 'ActiveScripting' THEN 'ActiveX Script' WHEN 'CmdExec' THEN 'Operating system (CmdExec)' WHEN 'PowerShell' THEN 'PowerShell' WHEN 'Distribution' THEN 'Replication Distributor' WHEN 'Merge' THEN 'Replication Merge' WHEN 'QueueReader' THEN 'Replication Queue Reader' WHEN 'Snapshot' THEN 'Replication Snapshot' WHEN 'LogReader' THEN 'Replication Transaction-Log Reader' WHEN 'ANALYSISCOMMAND' THEN 'SQL Server Analysis Services Command' WHEN 'ANALYSISQUERY' THEN 'SQL Server Analysis Services Query' WHEN 'SSIS' THEN 'SQL Server Integration Services Package' WHEN 'TSQL' THEN 'Transact-SQL script (T-SQL)' ELSE sJSTP.subsystem END [StepType], [sPROX].[name] [RunAs], [sJSTP].[database_name] [Database], CASE [sJSTP].[on_success_action] WHEN 1 THEN 'Quit the job reporting success' WHEN 2 THEN 'Quit the job reporting failure' WHEN 3 THEN 'Go to the next step' WHEN 4 THEN 'Go to Step: ' + QUOTENAME(CAST([sJSTP].[on_success_step_id] AS VARCHAR(3))) + ' ' + [sOSSTP].[step_name] END [OnSuccessAction], [sJSTP].[retry_attempts] [RetryAttempts], [sJSTP].[retry_interval] [RetryInterval (Minutes)], CASE [sJSTP].[on_fail_action] WHEN 1 THEN 'Quit the job reporting success' WHEN 2 THEN 'Quit the job reporting failure' WHEN 3 THEN 'Go to the next step' WHEN 4 THEN 'Go to Step: ' + QUOTENAME(CAST([sJSTP].[on_fail_step_id] AS VARCHAR(3)))+ ' ' + [sOFSTP].[step_name] END [OnFailureAction] FROM [msdb].[dbo].[sysjobsteps] [sJSTP] INNER JOIN [msdb].[dbo].[sysjobs] [sJOB] ON [sJSTP].[job_id] = [sJOB].[job_id] LEFT JOIN [msdb].[dbo].[sysjobsteps] [sOSSTP] ON [sJSTP].[job_id] = [sOSSTP].[job_id] AND [sJSTP].[on_success_step_id] = [sOSSTP].[step_id] LEFT JOIN [msdb].[dbo].[sysjobsteps] [sOFSTP] ON [sJSTP].[job_id] = [sOFSTP].[job_id] AND [sJSTP].[on_fail_step_id] = [sOFSTP].[step_id] LEFT JOIN [msdb].[dbo].[sysproxies] [sPROX] ON [sJSTP].[proxy_id] = [sPROX].[proxy_id] ORDER BY [JobName], [StepNo]";


		/// <summary>
		/// Operators 代理操作员
		/// </summary>
		public static string _sqlstrOperators = @"Select @@SERVERNAME ServerName,  name OperatorName, enabled IsEnabled,CASE WHEN last_email_date=0 or last_email_time=0 THEN 'Never' ELSE CAST(last_email_date AS varchar(12)) + CAST(last_email_time AS varchar(12))END LastEmailSent 
					from msdb..sysoperators";



		/// <summary>
		/// 
		///警告
		/// </summary>
		public static string _sqlstrAlerts = @"SELECT @@SERVERNAME ServerName, name AlertName,event_source EventSource,event_category_id EventCategoryId,event_id EventId,message_id MessageId,severity Severity,enabled IsEnabled,delay_between_responses DelayBetweenResponses,CASE WHEN last_occurrence_date=0 or last_occurrence_time=0 THEN 'Never' ELSE CAST(last_occurrence_date AS varchar(12)) + CAST(last_occurrence_time AS varchar(12))END LastOccurence,CASE WHEN last_response_date=0 or last_response_time=0 THEN 'Never' ELSE CAST(last_response_date AS varchar(12)) + CAST(last_response_time AS varchar(12))END LastNotification,notification_message NotificationMessage,include_event_description IncludeEventDescription,database_name DatabaseName,event_description_keyword EventDescriptionkeyword,performance_condition PerformanceCondition,category_id CategoryId 
					FROM msdb..sysalerts";

		/// <summary>
		/// ///加载到服务器的模块
		/// </summary>
		public static string _sqlstrLoadedModules = @"SELECT @@SERVERNAME ServerName, [description] ModuleName ,[name] ModulePath ,[company] Company, [file_version] FileVersion,[product_version] ProductVersion FROM sys.dm_os_loaded_modules";

		/// <summary>
		/// 内存性能指标
		/// </summary>
		public static string _sqlstrPerfMemory= @"	DECLARE @p1 int, @p2 int, @p3 int,@p4 datetime
		SELECT @p1 = cntr_value FROM sys.dm_os_performance_counters c
		  where c.object_name like '%Buffer Manager%' AND c.counter_name like '%Page Life Expectancy%'

		  SELECT @p2 = cntr_value / 1024  FROM sys.dm_os_performance_counters c
	  
		  where c.object_name like '%Memory Manager%' AND c.counter_name like '%Total Server Memory%'

		  SELECT @p3 = cntr_value / 1024 FROM sys.dm_os_performance_counters c
	  
		  where c.object_name like '%Memory Manager%' AND c.counter_name like '%Target Server Memory%'

		  SELECT @p4 = sqlserver_start_time from sys.dm_os_sys_info
	  
		  ; with cte as (SELECT database_id, count(*)*8/1024 BufferSize_MB FROM sys.dm_os_buffer_descriptors
			WHERE database_id<> 32767 GROUP BY database_id)
	SELECT  
		@@SERVERNAME ServerName,
		@p1 PageLifeExpectancy,
		@p2 TotalServerMemory_MB,
		@p3 TargetServerMemory_MB,
		@p4 LastSQLStartTime,
		DATEDIFF(DAY, @p4, GETDATE()) DaysRunning,
		DB_NAME(database_id) DBName, BufferSize_MB
	FROM cte ";


		/// <summary>
		/// 一小时内CPU指标
		/// </summary>
		public static string _sqlstrPerfCPU1h = @"DECLARE @ts_now BIGINT
		SELECT @ts_now = cpu_ticks / (cpu_ticks/ms_ticks) from sys.dm_os_sys_info;
		SELECT  @@SERVERNAME ServerName,
				DATEADD(ms, -1 * (@ts_now - [timestamp]), GETDATE())  AS EventTime,
				CPUUsage_SQL,
				100 - SystemIdle - CPUUsage_SQL                AS CPUUsage_Other,
				100 - SystemIdle CPUUsage_Total
		FROM (SELECT record.value('(./Record/@id)[1]', 'int') AS Record_ID,
				record.value('(./Record/SchedulerMonitorEvent/SystemHealth/SystemIdle)[1]', 'int')           AS SystemIdle,
				record.value('(./Record/SchedulerMonitorEvent/SystemHealth/ProcessUtilization)[1]', 'int')   AS CPUUsage_SQL,
				timestamp AS timestamp
			FROM (SELECT timestamp, CONVERT(XML, record) AS record
				FROM sys.dm_os_ring_buffers WHERE ring_buffer_type = N'RING_BUFFER_SCHEDULER_MONITOR' AND record LIKE '%<SystemHealth>%'
				) AS x
			) AS y ORDER BY Record_ID DESC";

		/// <summary>
		/// ///磁盘延时
		/// </summary>
		public static string _sqlstrDrive_Latency = @"SELECT @@SERVERNAME AS servername,tab.[Drive], tab.volume_mount_point AS [Volume Mount Point], 
		CASE 
			WHEN num_of_reads = 0 THEN 0 
			ELSE (io_stall_read_ms/num_of_reads) 
		END AS [Read Latency],
		CASE 
			WHEN num_of_writes = 0 THEN 0 
			ELSE (io_stall_write_ms/num_of_writes) 
		END AS [Write Latency],
		CASE 
			WHEN (num_of_reads = 0 AND num_of_writes = 0) THEN 0 
			ELSE (io_stall/(num_of_reads + num_of_writes)) 
		END AS [Overall Latency],
		CASE 
			WHEN num_of_reads = 0 THEN 0 
			ELSE (num_of_bytes_read/num_of_reads) 
		END AS [Avg Bytes/Read],
		CASE 
			WHEN num_of_writes = 0 THEN 0 
			ELSE (num_of_bytes_written/num_of_writes) 
		END AS [Avg Bytes/Write],
		CASE 
			WHEN (num_of_reads = 0 AND num_of_writes = 0) THEN 0 
			ELSE ((num_of_bytes_read + num_of_bytes_written)/(num_of_reads + num_of_writes)) 
		END AS [Avg Bytes/Transfer]
	FROM (SELECT LEFT(UPPER(mf.physical_name), 2) AS Drive, SUM(num_of_reads) AS num_of_reads,
					SUM(io_stall_read_ms) AS io_stall_read_ms, SUM(num_of_writes) AS num_of_writes,
					SUM(io_stall_write_ms) AS io_stall_write_ms, SUM(num_of_bytes_read) AS num_of_bytes_read,
					SUM(num_of_bytes_written) AS num_of_bytes_written, SUM(io_stall) AS io_stall, vs.volume_mount_point 
			FROM sys.dm_io_virtual_file_stats(NULL, NULL) AS vfs
			INNER JOIN sys.master_files AS mf WITH (NOLOCK)
			ON vfs.database_id = mf.database_id AND vfs.file_id = mf.file_id
			CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.[file_id]) AS vs 
			GROUP BY LEFT(UPPER(mf.physical_name), 2), vs.volume_mount_point) AS tab
	ORDER BY [Overall Latency] OPTION (RECOMPILE); ";


		/// <summary>
		/// 文件延时
		/// </summary>
		public static string _sqlstrFile_Level_Latency = @"SELECT @@SERVERNAME AS servername,DB_NAME(fs.database_id) AS [Database Name], CAST(fs.io_stall_read_ms/(1.0 + fs.num_of_reads) AS NUMERIC(10,1)) AS [avg_read_latency_ms],
		CAST(fs.io_stall_write_ms/(1.0 + fs.num_of_writes) AS NUMERIC(10,1)) AS [avg_write_latency_ms],
		CAST((fs.io_stall_read_ms + fs.io_stall_write_ms)/(1.0 + fs.num_of_reads + fs.num_of_writes) AS NUMERIC(10,1)) AS [avg_io_latency_ms],
		CONVERT(DECIMAL(18,2), mf.size/128.0) AS [File Size (MB)], mf.physical_name, mf.type_desc, fs.io_stall_read_ms, fs.num_of_reads, 
		fs.io_stall_write_ms, fs.num_of_writes, fs.io_stall_read_ms + fs.io_stall_write_ms AS [io_stalls], fs.num_of_reads + fs.num_of_writes AS [total_io]  
		FROM sys.dm_io_virtual_file_stats(null,null) AS fs
		INNER JOIN sys.master_files AS mf WITH (NOLOCK)
		ON fs.database_id = mf.database_id
		AND fs.[file_id] = mf.[file_id]
		ORDER BY avg_io_latency_ms DESC OPTION (RECOMPILE); ";




		/// <summary>
		/// 存储过程计划性能统计
		/// </summary>
		public static string _sqlstrExpensive_Procedure_Stats = @"DECLARE @ProductVersion NVARCHAR(128)
					SET @ProductVersion = CAST(SERVERPROPERTY ('ProductVersion') AS NVARCHAR(128))

					IF CAST(LEFT(@ProductVersion, CHARINDEX('.',@ProductVersion)-1) AS INT) > 8
					BEGIN
						IF OBJECT_ID('tempdb..#procedure_stats') IS NOT NULL
						begin
							drop table #procedure_stats
						end
						CREATE table #procedure_stats
						(
							DatabaseName			nvarchar(256),
							Procedure_Name			nvarchar(256),
							AverageLogicalReads		bigint,
							AverageLogicalWrites	bigint,
							AveragePhysicalReads	bigint,
							AverageRunTimeSeconds	numeric,
							execution_count			bigint,
							last_worker_time		bigint,
							last_physical_reads		bigint,
							total_logical_writes	bigint,
							last_logical_writes		bigint,
							last_logical_reads		bigint,
							last_elapsed_time		bigint,
							total_physical_reads	bigint,
							total_logical_reads		bigint,
						--	plan_handle				varbinary(64),
						--	sql_handle				varbinary(64),
							cached_time				datetime,
							last_execution_time		datetime,
							total_worker_time        BIGINT
						)
	
	
						--make sure that the context the statement is ran in is not in 2000 compat mode.
					DECLARE @DBContext SYSNAME, @SQL NVARCHAR(MAX)
					SELECT TOP 1 @DBContext = name 
					FROM sys.databases
					WHERE compatibility_level > 80
					EXECUTE sp_msforeachdb 'use [?];
						INSERT INTO #procedure_stats
						SELECT TOP 50
								db_name() as DatabaseName,
								Object_name(q.object_id) as Procedure_Name, 
								AverageLogicalReads		= total_logical_reads/execution_count,
								AverageLogicalWrites	= total_logical_writes/execution_count,
								AveragePhysicalReads	= total_physical_reads/execution_count,
								AverageRunTimeSeconds	= (total_elapsed_time/1000000.0)/execution_count,
								execution_count,
								last_worker_time,
								last_physical_reads,
								total_logical_writes,
								last_logical_writes,
								last_logical_reads,
								last_elapsed_time,
								total_physical_reads,
								total_logical_reads,
								--plan_handle,
								--sql_handle,
								cached_time,
								last_execution_time,
								total_worker_time
							FROM
								sys.dm_exec_procedure_stats q 
								where database_id=db_id() '
	    
						SELECT @@SERVERNAME ServerName,* FROM #procedure_stats 
						ORDER BY total_worker_time  DESC
					END";


		/// <summary>
		/// 触发器执行性能统计
		/// </summary>
		public static string _sqlstrExpensive_Trigger_Stats = @"
					SET NOCOUNT ON
					DECLARE @ProductVersion NVARCHAR(128)
					SET @ProductVersion = CAST(SERVERPROPERTY ('ProductVersion') AS NVARCHAR(128))

					IF CAST(LEFT(@ProductVersion, CHARINDEX('.',@ProductVersion)-1) AS INT) > 8
					BEGIN
						IF OBJECT_ID('tempdb..#Trigger_Stats') IS NOT NULL
						begin
							drop table #Trigger_Stats
						end
						CREATE table #Trigger_Stats
						(
							DatabaseName			nvarchar(256),
							Trigger_Name			nvarchar(256),
							AverageLogicalReads		bigint,
							AverageLogicalWrites	bigint,
							AveragePhysicalReads	bigint,
							AverageRunTimeSeconds	numeric,
							execution_count			bigint,
							last_worker_time		bigint,
							last_physical_reads		bigint,
							total_logical_writes	bigint,
							last_logical_writes		bigint,
							last_logical_reads		bigint,
							last_elapsed_time		bigint,
							total_physical_reads	bigint,
							total_logical_reads		bigint,
							--plan_handle				varbinary(64),
							--sql_handle				varbinary(64),
							cached_time				datetime,
							last_execution_time		datetime,
							total_worker_time        BIGINT
						)
	
	
						--make sure that the context the statement is ran in is not in 2000 compat mode.
					DECLARE @DBContext SYSNAME, @SQL NVARCHAR(MAX)
					SELECT TOP 1 @DBContext = name 
					FROM sys.databases
					WHERE compatibility_level > 80
					EXECUTE sp_msforeachdb 'use [?];
						INSERT INTO #Trigger_Stats
						SELECT TOP 50
								db_name() as DatabaseName,
								Object_name(q.object_id) as Trigger_Name, 
								AverageLogicalReads		= total_logical_reads/execution_count,
								AverageLogicalWrites	= total_logical_writes/execution_count,
								AveragePhysicalReads	= total_physical_reads/execution_count,
								AverageRunTimeSeconds	= (total_elapsed_time/1000000.0)/execution_count,
								execution_count,
								last_worker_time,
								last_physical_reads,
								total_logical_writes,
								last_logical_writes,
								last_logical_reads,
								last_elapsed_time,
								total_physical_reads,
								total_logical_reads,
								--plan_handle,
								--sql_handle,
								cached_time,
								last_execution_time,
								total_worker_time
							FROM
								sys.dm_exec_trigger_stats q 
								where database_id=db_id() '
	    
						SELECT @@SERVERNAME ServerName,* FROM #Trigger_Stats 
						ORDER BY total_worker_time  DESC
					END  ";



		/// <summary>
		/// 休眠的会话
		/// </summary>
		public static string _sqlstrSleeping_sessions = @"SET NOCOUNT ON
				select @@SERVERNAME ServerName,count(session_id) as session_id,[host_name]
				from sys.dm_exec_sessions
				where status='sleeping'
				and session_id >50 
				group by [host_name]";



		/// <summary>
		/// 休眠会话跟踪
		/// </summary>
		public static string _sqlstrSleeping_sessions_tran = @"SET NOCOUNT ON
							select @@SERVERNAME ServerName,
								   GETDATE() as datacollection_time,
								   c.session_id,
								   s.login_time,
								   s.host_name,program_name,
								   s.client_interface_name,
								   s.login_name,
								   s.nt_domain,
								   s.nt_user_name,
								   s.last_request_end_time,
								   s.is_user_process,
								   s.open_transaction_count,
								   DATEDIFF(MINUTE,s.last_request_end_time,GETDATE()) as sleepingtime_in_minutes,
								   substring(sh.text,1,200) as sqltext
							from sys.dm_exec_connections c
							inner join sys.dm_exec_sessions s on c.session_id=s.session_id
							outer apply sys.dm_exec_sql_text(c.most_recent_sql_handle) sh
							where status='sleeping'
							and s.session_id >50
							and s.open_transaction_count > 0
							option(recompile)";


		/// <summary>
		/// tempdb冲突信息
		/// </summary>
		 public static string _sqlstrTemp_db_Contention_Info = @"select  @@SERVERNAME ServerName,session_id, wait_type,wait_duration_ms,   resource_description 
						from    sys.dm_os_waiting_tasks
						where   wait_type like 'PAGE%LATCH_%' and
						resource_description like '2:%'";


		/// <summary>
		/// 当前运行查询
		/// </summary>
		public static string _sqlstrCurrent_running_queries = @"SET NOCOUNT ON
					SELECT 
					 @@SERVERNAME ServerName,
					 db_name(req.database_id) as Database_Name,
					 object_name(ST.objectid, st.dbid) 'ObjectName' ,
					 req.*, 
					substring(REPLACE (REPLACE (SUBSTRING (ST.text, (req.statement_start_offset/2) + 1 , 
					((CASE statement_end_offset WHEN -1 THEN DATALENGTH(ST.text) ELSE req.statement_end_offset

				   END - req.statement_start_offset)/2) + 1) , CHAR(10), ' '), CHAR(13), ' '), 1, 2000) AS statement_text
				FROM sys.dm_exec_requests AS req
				CROSS APPLY sys.dm_exec_sql_text(req.sql_handle) as ST
				WHERE req.session_id >50
				and req.session_id<> @@SPID
				ORDER BY cpu_time desc";
		
		/// <summary>
		/// 质疑的页
		/// </summary>
		public static string _sqlstrSuspectpages = @"select @@SERVERNAME ServerName,* from msdb..suspect_pages";




		/// <summary>
		/// 执行计划统计
		/// </summary>
		 public static string _sqlstrPlan_Count = @"SELECT @@SERVERNAME ServerName,objtype,count(*) as plancount
					FROM sys.dm_exec_cached_plans AS p
					group by objtype
					order by plancount desc
					option(recompile)";

		/// <summary>
		/// 数据库大表信息（超过1G）
		/// </summary>
		public static string _sqlstrDatabase_Table_Size_Info = @"IF OBJECT_ID('tempdb..#Table_Information') IS NOT NULL
						begin
							drop table #Table_Information
					end
					CREATE table #Table_Information
					(
						DatabaseName nvarchar(256),
						TableName nvarchar(256),
						SchemaName nvarchar(100),
						RowCounts BIGINT,
						TotalSpaceKB        BIGINT,
						UsedSpaceKB BIGINT,
						UnusedSpaceKB       BIGINT 

					)
					EXECUTE sp_msforeachdb 'use [?]
					INSERT INTO #Table_Information
					SELECT
						db_name() as DatabaseName,
						t.NAME AS TableName,
						s.Name AS SchemaName,
						p.rows AS RowCounts,
						SUM(a.total_pages) * 8 AS TotalSpaceKB,
						SUM(a.used_pages) * 8 AS UsedSpaceKB,
						(SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB
					FROM
						sys.tables t
					INNER JOIN
						sys.indexes i ON t.OBJECT_ID = i.object_id
					INNER JOIN
						sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
					INNER JOIN
						sys.allocation_units a ON p.partition_id = a.container_id
					LEFT OUTER JOIN
						sys.schemas s ON t.schema_id = s.schema_id
					WHERE
						t.type= ''U''
					GROUP BY
						t.Name, s.Name, p.Rows
					having (SUM(a.total_pages)* 8)/1024.0 > 1.0
						'

					select @@SERVERNAME ServerName,* from #Table_Information order by DatabaseName,TotalSpaceKB desc";

		/// <summary>
		/// 索引状态统计
		/// </summary>
		public static string _sqlstrStatistics_Information = @"
									IF OBJECT_ID('tempdb..#Statistics_Information') IS NOT NULL
									begin
										drop table #Statistics_Information
									end
									CREATE table #Statistics_Information
									(
										DatabaseName	nvarchar(256),
										object_name		nvarchar(514),
										stats_id		int	,
										index_or_stats_name	nvarchar(256),
										stat_source	varchar(5),
										total_rows	bigint,
										filtered_rows	bigint,
										filter_definition	nvarchar(500),
										sampled_rows	bigint,
										sampled_percent	float,
										last_updated	datetime2,
										num_modifications	bigint,
										hg_steps	int,
										persisted_sample	float,
										no_recompute	bit,
										is_temporary	bit,
										is_incremental	bit
									)
									EXECUTE sp_msforeachdb 'use [?];
									if(''?'' not in (''master'',''model'',''tempdb'',''msdb''))
									begin
										INSERT INTO #Statistics_Information
										select db_name() as DatabaseName,SCHEMA_NAME(o.schema_id) + ''.'' + o.name AS object_name, p.stats_id,
										   s.name AS index_or_stats_name,
										   CASE 
														 WHEN s.auto_created = 1 THEN ''Auto''
														 WHEN s.user_created = 1 THEN ''User'' 
														 ELSE ''Index'' 
										   END AS stat_source,
										   p.[rows] AS total_rows, 
										   p.[rows] - p.unfiltered_rows as filtered_rows, 
										   s.filter_definition,
										   p.rows_sampled AS sampled_rows,
										   (CAST(p.rows_sampled as float)/ p.rows) * 100.0 AS [sampled_percent],
										   CAST(p.last_updated AS DATETIME2(0)) AS last_updated, 
										   p.modification_counter AS num_modifications,
										   p.steps AS hg_steps, 
										   p.persisted_sample_percent AS persisted_sample, 
										   no_recompute,
										   s.is_temporary, s.is_incremental
									FROM sys.stats AS s
									INNER JOIN sys.objects AS o ON s.object_id = o.object_id
									CROSS APPLY sys.dm_db_stats_properties(o.object_id, s.stats_id) AS p
									--WHERE o.[name] = ''<table_name>''
									end
									'
									SELECT @@SERVERNAME ServerName,* FROM #Statistics_Information
									ORDER BY DatabaseName

									DROP TABLE #Statistics_Information
									";
		
		/// <summary>
		/// 弃用的功能
		/// </summary>
		public static string _sqlstrDeprecated_Features = @"SELECT * FROM sys.dm_os_performance_counters   
									WHERE object_name LIKE '%Deprecated Features%'
									and cntr_value >0; ";


		/// <summary>
		/// 内存延迟
		/// </summary>
		public static string _sqlstrMemory_Grants_Pending = @"SELECT @@SERVERNAME ServerName, RTRIM([object_name]) AS [ObjectName], cntr_value AS [Memory Grants Pending]
					FROM sys.dm_os_performance_counters WITH (NOLOCK)
					WHERE [object_name] LIKE N'%Memory Manager%' 
					AND counter_name = N'Memory Grants Pending' OPTION (RECOMPILE);";



		/// <summary>
		/// 延迟的IO请求
		/// </summary>
		public static string _sqlstrPending_IO_Requests = @"SELECT @@SERVERNAME ServerName,Runtime = GETDATE(), *
					FROM sys.dm_io_pending_io_requests";

		///页生命周期
		public static string _sqlstrPage_Life_Expectancy = @"SELECT 
					@@SERVERNAME ServerName, RTRIM([object_name]) AS [ObjectName], 
						   instance_name, cntr_value AS [PageLifeExpectancy]
					FROM sys.dm_os_performance_counters WITH (NOLOCK)
					WHERE [object_name] LIKE N'%Buffer Node%' -- Handles named instances
					AND counter_name = N'Page life expectancy' OPTION (RECOMPILE);";


		/// <summary>
		/// 数据库CheckDB检查时间
		/// </summary>
		public static string _sqlstrLastCheckDBDate = @"	IF OBJECT_ID('tempdb..#temp') IS NOT NULL
				DROP TABLE #temp;
				CREATE TABLE #temp
				(
				  ParentObject VARCHAR(255),
				  [Object] VARCHAR(255),
				  Field VARCHAR(255),
				  [Value] VARCHAR(255),
				  DBName VARCHAR(255)
				);

				EXECUTE sp_msforeachdb '
				INSERT INTO #temp (ParentObject, Object, Field, Value)
				EXEC(''DBCC DBINFO ( ''''?'''') WITH TABLERESULTS'')

				UPDATE #temp
				SET DBName = ''?''
				WHERE DBName IS NULL';

				SELECT DISTINCT 
					@@SERVERNAME ServerName,
					DBName,
					LastDBCCDate = CAST(Value AS DATETIME)
				FROM
					#temp
				WHERE
					Field = 'dbi_dbccLastKnownGood';";

		/// <summary>
		/// 闩锁等待统计信息
		/// </summary>
		public static string _sqlstrlatch_wait_stats = @"SELECT @@SERVERNAME ServerName,AvgWaitTimeMS = CASE WHEN wait_time_ms = 0 THEN 0 ELSE waiting_requests_count/wait_time_ms END,*
									FROM sys.dm_os_latch_stats
									ORDER BY wait_time_ms DESC;";
		/// <summary>
		/// 活动状态的内存分配TOP100

		/// </summary>
		public static string _sqlstrMemoryClerks_Info = @"SELECT top 100 @@SERVERNAME ServerName,[type]
							  ,[name]
							  ,[memory_node_id]
							  ,[pages_kb]
							  ,[virtual_memory_reserved_kb]
							  ,[virtual_memory_committed_kb]
							  ,[awe_allocated_kb]
							  ,[shared_memory_reserved_kb]
							  ,[shared_memory_committed_kb]
							  ,[page_size_in_bytes]   
							  ,[parent_memory_broker_type] FROM sys.dm_os_memory_clerks
							order by pages_kb desc";




		/// <summary>
		/// 内存管理器SQL Server内部分配
		/// </summary>
		public static string _sqlstrMemoryBrokers_Info = @"SELECT top 100 @@SERVERNAME ServerName,* FROM sys.dm_os_memory_brokers
						order by allocations_kb desc";

		/// <summary>
		/// 索引缺失信息
		/// </summary>
		public static string _sqlstrMissing_Index_Information = @"
SET NOCOUNT ON
DECLARE @ProductVersion NVARCHAR(128)
SET @ProductVersion = CAST(SERVERPROPERTY ('ProductVersion') AS NVARCHAR(128))

IF CAST(LEFT(@ProductVersion, CHARINDEX('.',@ProductVersion)-1) AS INT) > 8
BEGIN
	IF OBJECT_ID('tempdb..#missing_index_Stats') IS NOT NULL
	begin
		drop table #missing_index_Stats
	end
	CREATE TABLE #missing_index_Stats
	(

		DatabaseName		nvarchar(256),
		IndexImpact			float,
		LastUserSeek		datetime,
		FullObjectName		nvarchar(200),
		TableName			nvarchar(200),
		EqualityColumns		nvarchar(4000),
		InequalityColumns	nvarchar(4000),
		IncludedColumns		nvarchar(4000),
		Compiles			bigint,
		Seeks				bigint,
		UserCost			float,
		UserImpact			float,
		ProposedIndex		VARCHAR(8000)

	)

	EXECUTE sp_msforeachdb 'use [?];
	if(''?''  not in (''master'',''model'',''tempdb'',''msdb''))
	begin
			 
		INSERT INTO #missing_index_Stats
		SELECT
			DatabaseName = DB_NAME(id.database_id),
			IndexImpact = user_seeks * avg_total_user_cost * (avg_user_impact * 0.5),
			LastUserSeek = groupstats.last_user_seek,
			FullObjectName = id.[statement],
			TableName = REPLACE(REPLACE(REVERSE(LEFT(REVERSE(id.[statement]), CHARINDEX(''.'', REVERSE(id.[statement]))-1)),''['',''''), '']'',''''),
			EqualityColumns = id.equality_columns,
			InequalityColumns = id.inequality_columns,
			IncludedColumns = id.included_columns,
			Compiles = groupstats.unique_compiles,
			Seeks = groupstats.user_seeks,
			UserCost = groupstats.avg_total_user_cost,
			UserImpact = groupstats.avg_user_impact,
					''CREATE INDEX [IX_'' + OBJECT_NAME(id.[object_id], db.[database_id]) + ''_'' + REPLACE(REPLACE(REPLACE(ISNULL(id.[equality_columns], ''''), '', '', ''_''), ''['', ''''), '']'', '''') + CASE
					WHEN id.[equality_columns] IS NOT NULL
					AND id.[inequality_columns] IS NOT NULL
					THEN ''_''
					ELSE ''''
					END + REPLACE(REPLACE(REPLACE(ISNULL(id.[inequality_columns], ''''), '', '', ''_''), ''['', ''''), '']'', '''') + ''_''+ LEFT(CAST(NEWID() AS [nvarchar](64)), 5) + '']'' + '' ON '' + id.[statement] + '' ('' + ISNULL(id.[equality_columns], '''') + CASE
					WHEN id.[equality_columns] IS NOT NULL
					AND id.[inequality_columns] IS NOT NULL
					THEN '',''
					ELSE ''''
					END + ISNULL(id.[inequality_columns], '''') + '')'' + ISNULL('' INCLUDE ('' + id.[included_columns] + '')'', '''') AS [ProposedIndex]
					FROM sys.dm_db_missing_index_group_stats AS groupstats
					INNER JOIN sys.dm_db_missing_index_groups AS groups   ON groupstats.group_handle = groups.index_group_handle
					INNER JOIN sys.dm_db_missing_index_details AS id ON groups.index_handle = id.index_handle
					INNER JOIN [sys].[databases] db WITH (NOLOCK) ON db.[database_id] = id.[database_id]
					WHERE  db.[database_id] = DB_ID()
					end
			'
		SELECT @@SERVERNAME ServerName,* FROM #missing_index_Stats
		ORDER BY
		IndexImpact DESC
end
";

		/// <summary>
		/// 查询优化器SQL Server的详细统计信息
		/// </summary>
		public static string _sqlstrOptimizer_information = @"SELECT  @@SERVERNAME ServerName,*
						FROM sys.dm_exec_query_optimizer_info";

		/// <summary>
		/// TempDB使用状况
		/// </summary>
		public static string _sqlstrTemp_DB_Usage = @"SELECT TOP 50 @@SERVERNAME ServerName,
						 t1.session_id,
						 t1.request_id,
						 t1.task_alloc,
						 t1.task_dealloc,
 						 (SELECT SUBSTRING (text, t2.statement_start_offset/2 + 1,
							(CASE WHEN statement_end_offset = -1
							   THEN LEN(CONVERT(nvarchar(MAX),text)) * 2
							   ELSE statement_end_offset
							END - t2.statement_start_offset)/2)
						 FROM sys.dm_exec_sql_text(sql_handle)) AS query_text
						FROM (SELECT session_id, request_id,
							SUM(internal_objects_alloc_page_count +
							user_objects_alloc_page_count) AS task_alloc,
							SUM(internal_objects_dealloc_page_count +
							user_objects_dealloc_page_count) AS task_dealloc
						 FROM sys.dm_db_task_space_usage
						 GROUP BY session_id, request_id) AS t1,
						sys.dm_exec_requests AS t2
						WHERE t1.session_id = t2.session_id AND
						(t1.request_id = t2.request_id) AND t1.session_id > 50
						ORDER BY t1.task_alloc DESC";




		/// <summary>
		/// 查询存储基本信息
		/// </summary>
		public static string _sqlstrQuery_Store_information = @"SET NOCOUNT ON
							DECLARE @ProductVersion NVARCHAR(128)
							SET @ProductVersion = CAST(SERVERPROPERTY ('ProductVersion') AS NVARCHAR(128))
							IF CAST(LEFT(@ProductVersion, CHARINDEX('.',@ProductVersion)-1) AS INT) > 12
							BEGIN 
									if OBJECT_ID('tempdb..#tmp_query_store') is not null
									drop table #tmp_query_store; 

										SELECT  
											DBName = DB_NAME(), *
										into  #tmp_query_store
									FROM sys.database_query_store_options WITH (NOLOCK) 
									OPTION (RECOMPILE); 
						
									insert into #tmp_query_store 
									EXECUTE sp_msforeachdb N'use [?];
											if(''?'' not in (''master'',''model'',''tempdb'',''msdb''))
											begin
											SELECT  
													DBName = DB_NAME(), * 
											FROM sys.database_query_store_options WITH (NOLOCK) 
											OPTION (RECOMPILE); 
											end
										'
									select @@SERVERNAME ServerName,* 
									from #tmp_query_store 

									drop table #tmp_query_store
								END";


		/// <summary>
		/// 查询存储统计信息
		/// </summary>
		public static string _sqlstrQuery_Store_Stats = @"SET NOCOUNT ON
						DECLARE @ProductVersion NVARCHAR(128)
						SET @ProductVersion = CAST(SERVERPROPERTY ('ProductVersion') AS NVARCHAR(128))
						IF CAST(LEFT(@ProductVersion, CHARINDEX('.',@ProductVersion)-1) AS INT) > 12
						BEGIN 
								if OBJECT_ID('tempdb..#tmp_query_store') is not null
								drop table #tmp_query_store; 
								create table #tmp_query_store
								(
									dbname						nvarchar(256),
									query_id					bigint,
 									query_hash_bigint			nvarchar(100),
									query_sql_text				nvarchar(2000),
									total_executions			bigint,
									total_row_count				float ,
									total_duration_sec			float ,
									avg_duration_sec			float ,
									max_duration_sec			float ,
									total_cpu_time_sec			float ,
									total_logical_io_reads_kb	float ,
									total_logical_io_writes_kb	float ,
									total_physical_io_reads_kb	float 
								)
								insert into #tmp_query_store 
								EXECUTE sp_msforeachdb N'use [?];
									 if(''?'' not in (''master'',''model'',''tempdb'',''msdb''))
									begin
										select top 200
												db_name() as dbname,
												max(p.query_id) as query_id,
 												cast(cast(q.query_hash as bigint) as nvarchar(50)) as query_hash_bigint,
												max(left(qt.query_sql_text, 2000)) as query_sql_text,
												sum(count_executions) as total_executions,
												sum(count_executions * avg_rowcount) as total_row_count,
												round(sum(count_executions * avg_duration) / 1000000, 2) as total_duration_sec,
												round(sum(count_executions * avg_duration) / sum(count_executions) / 1000000, 2) as avg_duration_sec,
												round(max(max_duration) / cast(1000000 as float), 2) as max_duration_sec,
												round(sum(count_executions * avg_cpu_time) / 1000000, 2) as total_cpu_time_sec,
												sum(count_executions * avg_logical_io_reads * 8) as total_logical_io_reads_kb,
												sum(count_executions * avg_logical_io_writes * 8) as total_logical_io_writes_kb,
												sum(count_executions * avg_physical_io_reads * 8) as total_physical_io_reads_kb
										from sys.query_store_runtime_stats as rs
										inner join sys.query_store_plan as p on rs.plan_id = p.plan_id
										inner join sys.query_store_query as q on p.query_id = q.query_id
										inner join sys.query_store_query_text as qt on q.query_text_id = qt.query_text_id
										where ((rs.first_execution_time >= getdate()-7 and rs.first_execution_time <= getdate()) or
												(rs.first_execution_time >= getdate()-7 and rs.first_execution_time <= getdate()))
												and ''?'' not in (''msdb'' ,''master'',''model'',''tempdb'')
										group by q.query_hash
									end
									'
								select @@SERVERNAME ServerName,* 
								from #tmp_query_store order by dbname,avg_duration_sec desc

								drop table #tmp_query_store
							END";

		/// <summary>
		/// 旋转锁等待信息
		/// </summary>
		public static string _sqlstrSpinlocks_information = @"select @@SERVERNAME ServerName,* from sys.dm_os_spinlock_stats";

		/// <summary>
		/// 内存不足异常
		/// </summary>
		public static string _sqlstrOOM_Exception = @"SELECT @@SERVERNAME ServerName,
				CASE WHEN x.[TIMESTAMP] BETWEEN -2147483648 AND 2147483647 AND si.ms_ticks BETWEEN -2147483648 AND 2147483647 THEN DATEADD(ms, x.[TIMESTAMP] - si.ms_ticks, GETDATE()) 
					ELSE DATEADD(s, ([TIMESTAMP]/1000) - (si.ms_ticks/1000), GETDATE()) END AS Event_Time,
					record.value('(./Record/OOM/Action)[1]', 'varchar(50)') AS [Action],
					record.value('(./Record/OOM/Resources)[1]', 'int') AS [Resources],
					record.value('(./Record/OOM/Task)[1]', 'varchar(20)') AS [Task],
					record.value('(./Record/OOM/Pool)[1]', 'int') AS [PoolID],
					rgrp.name AS [PoolName],
					record.value('(./Record/MemoryRecord/MemoryUtilization)[1]', 'bigint') AS [MemoryUtilPct],
					record.value('(./Record/MemoryRecord/TotalPhysicalMemory)[1]', 'bigint')/1024 AS [Total_Physical_Mem_MB],
					record.value('(./Record/MemoryRecord/AvailablePhysicalMemory)[1]', 'bigint')/1024 AS [Avail_Physical_Mem_MB],
					record.value('(./Record/MemoryRecord/AvailableVirtualAddressSpace)[1]', 'bigint')/1024 AS [Avail_VAS_MB],
					record.value('(./Record/MemoryRecord/TotalPageFile)[1]', 'bigint')/1024 AS [Total_Pagefile_MB],
					record.value('(./Record/MemoryRecord/AvailablePageFile)[1]', 'bigint')/1024 AS [Avail_Pagefile_MB]
				FROM (SELECT [TIMESTAMP], CONVERT(xml, record) AS record 
							FROM sys.dm_os_ring_buffers (NOLOCK)
							WHERE ring_buffer_type = N'RING_BUFFER_OOM') AS x
				CROSS JOIN sys.dm_os_sys_info si (NOLOCK)
				LEFT JOIN sys.resource_governor_resource_pools rgrp (NOLOCK) ON rgrp.pool_id = record.value('(./Record/OOM/Pool)[1]', 'int')
				--WHERE CASE WHEN x.[timestamp] BETWEEN -2147483648 AND 2147483648 THEN DATEADD(ms, x.[timestamp] - si.ms_ticks, GETDATE()) 
				--	ELSE DATEADD(s, (x.[timestamp]/1000) - (si.ms_ticks/1000), GETDATE()) END >= DATEADD(hh, -12, GETDATE())
				ORDER BY 1 DESC;";
		/// <summary>
		/// 索引使用统计
		/// </summary>
		public static string _sqlstrIndex_Usage = @"SET NOCOUNT ON
						if OBJECT_ID('tempdb..#Ind') is not null
						drop table #Ind; 
						create table #Ind
						(
							DatabaseName nvarchar(255), TableName nvarchar(255), IndexName nvarchar(255), indextype varchar(100),Indexsize_MB BIGINT,
							UserSeeks BIGINT, UserScans BIGINT, UserLookups BIGINT, UserUpdates BIGINT,
							LastRestart DATETIME, LastUserSeek DATETIME, LastUserScan DATETIME,
							LastUserLookup DATETIME, LastUserUpdate DATETIME, TableRows BIGINT
						);
						insert into #Ind
						EXECUTE sp_msforeachdb N'use [?];
						if(''?'' not in (''master'',''model'',''tempdb'',''msdb''))
						begin  
							SELECT
								''?'' as dbname,
								o.name AS tablename,
								i.name AS indexname,
								i.type_desc as indextype,
								indexspace.[Indexsize_MB],
								u.user_seeks,
								u.user_scans,
								u.user_lookups,
								u.user_updates,
								LastRestart = 
								(
									SELECT create_date 
									FROM sys.databases
									WHERE database_id = 2
								),    
								u.last_user_seek,
								u.last_user_scan,
								u.last_user_lookup,
								u.last_user_update, 
								x.TableRows
							FROM
								sys.dm_db_index_usage_stats u 
							JOIN sys.indexes i ON  	u.object_id = i.object_id AND u.index_id = i.index_id
							JOIN sys.objects o ON 	i.object_id = o.object_id
							LEFT JOIN
							(
								SELECT tn.OBJECT_ID as tblobj,ix.index_id as indexid,
								(SUM(sz.[used_page_count]) * 8.0)/1024.0 AS [Indexsize_MB]
								FROM sys.dm_db_partition_stats AS sz
								INNER JOIN sys.indexes AS ix ON sz.[object_id] = ix.[object_id] AND sz.[index_id] = ix.[index_id]
								INNER JOIN sys.tables tn ON tn.OBJECT_ID = ix.object_id
								GROUP BY tn.OBJECT_ID , ix.index_id
							)indexspace ON o.object_id=indexspace.tblobj AND indexspace.indexid=i.index_id
							LEFT JOIN 
							(
								 SELECT
									object_id,
									SUM(rows) AS TableRows
								 FROM
									sys.partitions  WITH(NOLOCK)
								 WHERE
									index_id = 1
								 GROUP BY
									object_id
							) x
							ON o.object_id = x.object_id
							WHERE
								o.type = ''u'' and 
								u.database_id = DB_ID()
						end
						' 
						select @@SERVERNAME ServerName,* 
						from #Ind order by DatabaseName";


		/// <summary>
		/// 索引碎片报告
		/// </summary>
		public static string _sqlstrIndex_Fragmentation = @"SET NOCOUNT ON
					if OBJECT_ID('tempdb..#Ind') is not null
					drop table #Ind; 
					create table #Ind
					(
						DatabaseName nvarchar(256),
						Schema_Name nvarchar(256),
						Object_Name nvarchar(256),
						Index_Name nvarchar(256),
						index_id int,
						index_type_desc nvarchar(120),
						avg_fragmentation_in_percent float,
						fragment_count bigint,
						page_count  bigint,
						fill_factor tinyint,
						has_filter  bit,
						filter_definition nvarchar(max),
						allow_page_locks bit
						, Indexsize_MB BIGINT
					);

					EXECUTE sp_msforeachdb N'use [?];
						insert into #Ind
						SELECT
							DB_NAME(ps.database_id) AS[Database Name], 
							SCHEMA_NAME(o.[schema_id]) AS[Schema Name],
							OBJECT_NAME(ps.OBJECT_ID) AS[Object Name], 
							i.[name] AS[Index Name],
							ps.index_id, 
							ps.index_type_desc, 
							ps.avg_fragmentation_in_percent, 
							ps.fragment_count, 
							ps.page_count, 
							i.fill_factor, 
							i.has_filter, 
							i.filter_definition, 
							i.[allow_page_locks] ,
							indexspace.[Indexsize_MB]
						FROM sys.dm_db_index_physical_stats(DB_ID(),NULL, NULL, NULL , N''LIMITED'') AS ps
						INNER JOIN sys.indexes AS i WITH(NOLOCK) ON ps.[object_id] = i.[object_id]  AND ps.index_id = i.index_id

					   INNER JOIN sys.objects AS o WITH (NOLOCK) ON i.[object_id] = o.[object_id]

					   LEFT JOIN

					   (
						   SELECT tn.OBJECT_ID as tblobj, ix.index_id as indexid,
						   (SUM(sz.[used_page_count])* 8.0)/1024.0 AS[Indexsize_MB]
						  FROM sys.dm_db_partition_stats AS sz
						  INNER JOIN sys.indexes AS ix ON sz.[object_id] = ix.[object_id] AND sz.[index_id] = ix.[index_id]

						  INNER JOIN sys.tables tn ON tn.OBJECT_ID = ix.object_id

						  GROUP BY tn.OBJECT_ID , ix.index_id

					  )indexspace ON o.object_id=indexspace.tblobj AND indexspace.indexid=i.index_id
					  WHERE ps.database_id = DB_ID()
						--AND ps.page_count > 2500 
						and ''?'' not in ( ''msdb'',''tempdb'',''master'',''mode'')
						ORDER BY ps.avg_fragmentation_in_percent DESC OPTION(RECOMPILE)
						' 

					select @@SERVERNAME ServerName,* 
					from #Ind order by DatabaseName  ";

		/// <summary>
		/// 等待信息
		/// </summary>
		public static string _sqlstrWaits_information = @"SET NOCOUNT ON
								;WITH [Waits] AS
								(SELECT
									[wait_type],
									[wait_time_ms] / 1000.0 AS [WaitS],
									([wait_time_ms] - [signal_wait_time_ms]) / 1000.0 AS [ResourceS],
									[signal_wait_time_ms] / 1000.0 AS [SignalS],
									[waiting_tasks_count] AS [WaitCount],
									100.0 * [wait_time_ms] / SUM ([wait_time_ms]) OVER() AS [Percentage],
									ROW_NUMBER() OVER(ORDER BY [wait_time_ms] DESC) AS [RowNum]
								FROM sys.dm_os_wait_stats
								WHERE [wait_type] NOT IN (
									N'BROKER_EVENTHANDLER', 
									N'BROKER_RECEIVE_WAITFOR', 
									N'BROKER_TASK_STOP', 
									N'BROKER_TO_FLUSH', 
									N'BROKER_TRANSMITTER', 
									N'CHECKPOINT_QUEUE', 
									N'CHKPT', 
									N'CLR_AUTO_EVENT', 
									N'CLR_SEMAPHORE', 
									N'CXCONSUMER', 
									N'DBMIRROR_DBM_EVENT', 
									N'DBMIRROR_EVENTS_QUEUE', 
									N'DBMIRROR_WORKER_QUEUE', 
									N'DBMIRRORING_CMD', 
									N'DIRTY_PAGE_POLL', 
									N'DISPATCHER_QUEUE_SEMAPHORE', 
									N'EXECSYNC', 
									N'FSAGENT', 
									N'FT_IFTS_SCHEDULER_IDLE_WAIT', 
									N'FT_IFTSHC_MUTEX', 
									N'HADR_CLUSAPI_CALL', 
									N'HADR_FILESTREAM_IOMGR_IOCOMPLETION', 
									N'HADR_LOGCAPTURE_WAIT', 
									N'HADR_NOTIFICATION_DEQUEUE', 
									N'HADR_TIMER_TASK', 
									N'HADR_WORK_QUEUE', 
									N'KSOURCE_WAKEUP', 
									N'LAZYWRITER_SLEEP', 
									N'LOGMGR_QUEUE', 
									N'MEMORY_ALLOCATION_EXT', 
									N'ONDEMAND_TASK_QUEUE', 
									N'PARALLEL_REDO_DRAIN_WORKER', 
									N'PARALLEL_REDO_LOG_CACHE', 
									N'PARALLEL_REDO_TRAN_LIST', 
									N'PARALLEL_REDO_WORKER_SYNC', 
									N'PARALLEL_REDO_WORKER_WAIT_WORK', 
									N'PREEMPTIVE_XE_GETTARGETSTATE', 
									N'PWAIT_ALL_COMPONENTS_INITIALIZED', 
									N'PWAIT_DIRECTLOGCONSUMER_GETNEXT', 
									N'QDS_PERSIST_TASK_MAIN_LOOP_SLEEP', 
									N'QDS_ASYNC_QUEUE', 
									N'QDS_CLEANUP_STALE_QUERIES_TASK_MAIN_LOOP_SLEEP',
									N'QDS_SHUTDOWN_QUEUE', 
									N'REDO_THREAD_PENDING_WORK', 
									N'REQUEST_FOR_DEADLOCK_SEARCH', 
									N'RESOURCE_QUEUE', 
									N'SERVER_IDLE_CHECK', 
									N'SLEEP_BPOOL_FLUSH', 
									N'SLEEP_DBSTARTUP', 
									N'SLEEP_DCOMSTARTUP', 
									N'SLEEP_MASTERDBREADY', 
									N'SLEEP_MASTERMDREADY', 
									N'SLEEP_MASTERUPGRADED', 
									N'SLEEP_MSDBSTARTUP', 
									N'SLEEP_SYSTEMTASK', 
									N'SLEEP_TASK',
									N'SLEEP_TEMPDBSTARTUP', 
									N'SNI_HTTP_ACCEPT', 
									N'SOS_WORK_DISPATCHER', 
									N'SP_SERVER_DIAGNOSTICS_SLEEP', 
									N'SQLTRACE_BUFFER_FLUSH', 
									N'SQLTRACE_INCREMENTAL_FLUSH_SLEEP', 
									N'SQLTRACE_WAIT_ENTRIES', 
									N'WAIT_FOR_RESULTS', 
									N'WAITFOR', 
									N'WAITFOR_TASKSHUTDOWN', 
									N'WAIT_XTP_RECOVERY', 
									N'WAIT_XTP_HOST_WAIT', 
									N'WAIT_XTP_OFFLINE_CKPT_NEW_LOG', 
									N'WAIT_XTP_CKPT_CLOSE', 
									N'XE_DISPATCHER_JOIN', 
									N'XE_DISPATCHER_WAIT', 
									N'XE_TIMER_EVENT' 
									)
								AND [waiting_tasks_count] > 0
								)
								SELECT @@SERVERNAME as instance_name,
									MAX ([W1].[wait_type]) AS [WaitType],
									CAST (MAX ([W1].[WaitS]) AS DECIMAL (16,2)) AS [Wait_S],
									CAST (MAX ([W1].[ResourceS]) AS DECIMAL (16,2)) AS [Resource_S],
									CAST (MAX ([W1].[SignalS]) AS DECIMAL (16,2)) AS [Signal_S],
									MAX ([W1].[WaitCount]) AS [WaitCount],
									CAST (MAX ([W1].[Percentage]) AS DECIMAL (5,2)) AS [Percentage],
									CAST ((MAX ([W1].[WaitS]) / MAX ([W1].[WaitCount])) AS DECIMAL (16,4)) AS [AvgWait_S],
									CAST ((MAX ([W1].[ResourceS]) / MAX ([W1].[WaitCount])) AS DECIMAL (16,4)) AS [AvgRes_S],
									CAST ((MAX ([W1].[SignalS]) / MAX ([W1].[WaitCount])) AS DECIMAL (16,4)) AS [AvgSig_S]
								FROM [Waits] AS [W1]
								INNER JOIN [Waits] AS [W2] ON [W2].[RowNum] <= [W1].[RowNum]
								GROUP BY [W1].[RowNum]
								HAVING SUM ([W2].[Percentage]) - MAX( [W1].[Percentage] ) < 95; ";


		
		/// <summary>
		/// 数据库文件报告
		/// </summary>
		
		public static string _sqlstrDB_File_Size_Info = @"
							CREATE TABLE #tempFileInformation
							(
								DBNAME          NVARCHAR(256),
								[FILENAME]      NVARCHAR(256),
								[TYPE]          NVARCHAR(120),
								FILEGROUPNAME   NVARCHAR(120),
								FILE_LOCATION   NVARCHAR(500),
								FILESIZE_MB     DECIMAL(10,2),
								USEDSPACE_MB    DECIMAL(10,2),
								FREESPACE_MB    DECIMAL(10,2),
								AUTOGROW_STATUS NVARCHAR(100)
							);
							DECLARE @SQL VARCHAR(2000)

							SELECT @SQL = '
									USE [?];
											INSERT INTO #tempFileInformation
											SELECT  
												DBNAME          =DB_NAME(),     
												[FILENAME]      =A.NAME,
												[TYPE]          = A.TYPE_DESC,
												FILEGROUPNAME   = fg.name,
												FILE_LOCATION   =a.PHYSICAL_NAME,
												FILESIZE_MB     = CONVERT(DECIMAL(10,2),A.SIZE/128.0),
												USEDSPACE_MB    = CONVERT(DECIMAL(10,2),(A.SIZE/128.0 - ((A.SIZE - CAST(FILEPROPERTY(A.NAME,''SPACEUSED'') AS INT))/128.0))),
												FREESPACE_MB    = CONVERT(DECIMAL(10,2),(A.SIZE/128.0 -  CAST(FILEPROPERTY(A.NAME,''SPACEUSED'') AS INT)/128.0)),
												AUTOGROW_STATUS = ''BY '' +CASE is_percent_growth when 0 then cast (growth/128 as varchar(10))+ '' MB - ''
																									when 1 then cast (growth as varchar(10)) + ''% - '' ELSE '''' END
																									+ CASE MAX_SIZE WHEN 0 THEN '' DISABLED '' 
																													WHEN -1 THEN '' UNRESTRICTED''
																													ELSE '' RESTRICTED TO '' + CAST(MAX_SIZE/(128*1024) AS VARCHAR(10)) + '' GB '' END
																								+ CASE IS_PERCENT_GROWTH WHEn 1 then '' [autogrowth by percent]'' else '''' end
									from sys.database_files A
									left join sys.filegroups fg on a.data_space_id = fg.data_space_id
									order by A.type desc,A.name
								;
								'
								EXEC sp_MSforeachdb @SQL;
    
								SELECT @@SERVERNAME as instance_name,dbSize.*,fg.*,d.log_reuse_wait_desc,d.recovery_model_desc,d.*
								FROM #tempFileInformation fg
								LEFT JOIN sys.databases d on fg.DBNAME = d.name
								CROSS APPLY
								(
									select dbname,
											sum(FILESIZE_MB) as [totalDBSize_MB],
											sum(FREESPACE_MB) as [DB_Free_Space_Size_MB],
											sum(USEDSPACE_MB) as [DB_Used_Space_Size_MB]
										from #tempFileInformation
										where  dbname = fg.dbname
										group by dbname
								)dbSize ;
							DROP TABLE #tempFileInformation";




		/// <summary>
		/// MaterDB内对象

		/// </summary>
		public static string _sqlstrObject_In_Master_DB = @"SELECT @@SERVERNAME ServerName,'Database_checks' AS [Category], 'User_Objects_in_master' AS [Information], ss.name AS [Schema_Name], sao.name AS [Object_Name], sao.[type_desc] AS [Object_Type], sao.create_date, sao.modify_date 
							FROM master.sys.all_objects sao
							INNER JOIN master.sys.schemas ss ON sao.[schema_id] = ss.[schema_id]
							WHERE sao.is_ms_shipped = 0
							AND sao.[type] IN ('AF','FN','P','IF','PC','TF','TR','T','V')
							ORDER BY sao.name, sao.type_desc;";



		/// <summary>
		/// 启动自动执行存储过程
		/// </summary>
		public static string _sqlstrStartup_Procedures = @"select @@SERVERNAME AS ServerName, S.name as schemaname, P.name as spname 
						FROM master.sys.procedures P 
						INNER JOIN master.sys.schemas S ON P.schema_id = S.schema_id 
						WHERE is_auto_executed = 1";


		/// <summary>
		/// 数据库级别权限
		/// </summary>
		public static string _sqlstrDB_Level_Permission = @"if OBJECT_ID('tempdb..#tmp_databases') is not null
								drop table #tmp_databases;
							if OBJECT_ID('tempdb..#Ind') is not null
								drop table #tmp_permission; 

							CREATE TAble #tmp_databases
							(
							id int identity(1,1),
							dbname varchar(200)
							);
							CREATE TABLE #tmp_permission
							(
								dbname VARCHAR(200),
								Permission VARCHAR(2000)
							)

							insert into #tmp_databases
							select name from sys.databases where is_read_only=0 and state_desc='ONLINE' --and name not in ('master','model','tempdb','msdb');

							DECLARE @min int=1,@max int =0
							select @max=count(*) from #tmp_databases

							while(@min <=@max)
							begin
							DECLARE @SQL VARCHAR(8000) = ''
							,@dbname varchar(200) = ''

							SELECT @dbname=dbname from #tmp_databases where id = @min

							SELECT @SQL=' USE ['+@dbname+']	
	
								INSERT INTO #tmp_permission
								select DB_NAME() AS dBNAME	, CASE WHEN perm.state <> ''W'' THEN perm.state_desc ELSE ''GRANT'' END
											+ SPACE(1) + perm.permission_name + SPACE(1)
											+ SPACE(1) + ''TO'' + SPACE(1) + QUOTENAME(usr.name) COLLATE database_default
											+ CASE WHEN perm.state <> ''W'' THEN SPACE(0) ELSE SPACE(1) + ''WITH GRANT OPTION'' END AS ''--Database Level Permissions''
											FROM sys.database_permissions AS perm
											INNER JOIN
											sys.database_principals AS usr
											ON perm.grantee_principal_id = usr.principal_id
											WHERE perm.major_id = 0
											ORDER BY perm.permission_name ASC, perm.state_desc ASC
								'
								PRINT @SQL
									EXEC (@SQL) 
									set @min = @min+1
								end		
								select @@SERVERNAME AS ServerName,* from #tmp_permission
								DROP TABLE #tmp_permission
								DROP TABLE #tmp_databases ";


		/// <summary>
		/// 数据库角色成员
		/// </summary>
		public static string _sqlstrDB_role_members = @"if OBJECT_ID('tempdb..#tmp_databases') is not null
							drop table #tmp_databases;
						if OBJECT_ID('tempdb..#Ind') is not null
							drop table #tmp_permission; 

						CREATE TAble #tmp_databases
						(
						id int identity(1,1),
						dbname varchar(200)
						);
						CREATE TABLE #tmp_permission
						(
							dbname VARCHAR(200),
							Permission VARCHAR(2000)
						)

						insert into #tmp_databases
						select name from sys.databases where is_read_only=0 and state_desc='ONLINE' --and name not in ('master','model','tempdb','msdb');

						DECLARE @min int=1,@max int =0
						select @max=count(*) from #tmp_databases

						while(@min <=@max)
						begin
						DECLARE @SQL VARCHAR(8000) = ''
						,@dbname varchar(200) = ''

						SELECT @dbname=dbname from #tmp_databases where id = @min

						SELECT @SQL=' USE ['+@dbname+']	
		
							INSERT INTO #tmp_permission
							select DB_NAME() AS dBNAME	, ''EXEC sp_addrolemember @rolename =''
							+ SPACE(1) + QUOTENAME(USER_NAME(rm.role_principal_id), '''''''') 
							+ '', @membername ='' + SPACE(1) + QUOTENAME(USER_NAME(rm.member_principal_id), '''''''') AS ''--Role Memberships''
							FROM sys.database_role_members AS rm
							ORDER BY rm.role_principal_id 
							'
							PRINT @SQL
								EXEC (@SQL) 
								set @min = @min+1
							end		
							select @@SERVERNAME AS ServerName,* from #tmp_permission
							DROP TABLE #tmp_permission
							DROP TABLE #tmp_databases ";

		/// <summary>
		/// 数据库对象权限
		/// </summary>
		public static string _sqlstrDB_object_permission = @"if OBJECT_ID('tempdb..#tmp_databases') is not null
								drop table #tmp_databases;
							if OBJECT_ID('tempdb..#Ind') is not null
								drop table #tmp_permission; 

							CREATE TAble #tmp_databases
							(
							id int identity(1,1),
							dbname varchar(200)
							);
							CREATE TABLE #tmp_permission
							(
								dbname VARCHAR(200),
								Permission VARCHAR(2000)
							)

							insert into #tmp_databases
							select name from sys.databases where is_read_only=0 and state_desc='ONLINE' --and name not in ('master','model','tempdb','msdb');

							DECLARE @min int=1,@max int =0
							select @max=count(*) from #tmp_databases

							while(@min <=@max)
							begin
							DECLARE @SQL VARCHAR(8000) = ''
							,@dbname varchar(200) = ''

							SELECT @dbname=dbname from #tmp_databases where id = @min

							SELECT @SQL=' USE ['+@dbname+']	
		
								INSERT INTO #tmp_permission
								select DB_NAME() AS dBNAME	,CASE WHEN perm.state != ''W'' THEN perm.state_desc ELSE ''GRANT'' END + SPACE(1) + 
								perm.permission_name + SPACE(1) + ''ON ''+ QUOTENAME(Schema_NAME(obj.schema_id)) + ''.'' 
								+ QUOTENAME(obj.name) collate Latin1_General_CI_AS_KS_WS 
								+ CASE WHEN cl.column_id IS NULL THEN SPACE(0) ELSE ''('' + QUOTENAME(cl.name) + '')'' END
								+ SPACE(1) + ''TO'' + SPACE(1) + QUOTENAME(usr.name)
								+ CASE WHEN perm.state <> ''W'' THEN SPACE(0) ELSE SPACE(1) + ''WITH GRANT OPTION'' END AS ''--Object Level Permissions''
								FROM sys.database_permissions AS perm
								INNER JOIN sys.objects AS obj ON perm.major_id = obj.[object_id]
								INNER JOIN sys.database_principals AS usr ON perm.grantee_principal_id = usr.principal_id
								LEFT JOIN sys.columns AS cl ON cl.column_id = perm.minor_id AND cl.[object_id] = perm.major_id 
								'

									EXEC (@SQL) 
									set @min = @min+1
								end		
								select @@SERVERNAME AS ServerName,* from #tmp_permission
								DROP TABLE #tmp_permission
								DROP TABLE #tmp_databases ";




		/// <summary>
		/// 失败的Job
		/// </summary>
		public static string _sqlstrFailed_Jobs = @"  SELECT   @@SERVERNAME ServerName,
								Job.instance_id
								,SysJobs.job_id
								,SysJobs.name as 'JOB_NAME'
								,SysJobSteps.step_name as 'STEP_NAME'
								,Job.run_status
								,Job.sql_message_id
								,Job.sql_severity
								,Job.message
								,Job.exec_date
								,Job.run_duration
								,Job.server
								,SysJobSteps.output_file_name
							FROM    (SELECT Instance.instance_id
								,DBSysJobHistory.job_id
								,DBSysJobHistory.step_id
								,DBSysJobHistory.sql_message_id
								,DBSysJobHistory.sql_severity
								,DBSysJobHistory.message
								,(CASE DBSysJobHistory.run_status
									WHEN 0 THEN 'Failed'
									WHEN 1 THEN 'Succeeded'
									WHEN 2 THEN 'Retry'
									WHEN 3 THEN 'Canceled'
									WHEN 4 THEN 'In progress'
								END) as run_status
								,((SUBSTRING(CAST(DBSysJobHistory.run_date AS VARCHAR(8)), 5, 2) + '/'
								+ SUBSTRING(CAST(DBSysJobHistory.run_date AS VARCHAR(8)), 7, 2) + '/'
								+ SUBSTRING(CAST(DBSysJobHistory.run_date AS VARCHAR(8)), 1, 4) + ' '
								+ SUBSTRING((REPLICATE('0',6-LEN(CAST(DBSysJobHistory.run_time AS varchar)))
								+ CAST(DBSysJobHistory.run_time AS VARCHAR)), 1, 2) + ':'
								+ SUBSTRING((REPLICATE('0',6-LEN(CAST(DBSysJobHistory.run_time AS VARCHAR)))
								+ CAST(DBSysJobHistory.run_time AS VARCHAR)), 3, 2) + ':'
								+ SUBSTRING((REPLICATE('0',6-LEN(CAST(DBSysJobHistory.run_time as varchar)))
								+ CAST(DBSysJobHistory.run_time AS VARCHAR)), 5, 2))) AS 'exec_date'
								,DBSysJobHistory.run_duration
								,DBSysJobHistory.retries_attempted
								,DBSysJobHistory.server
								FROM msdb.dbo.sysjobhistory DBSysJobHistory
								JOIN (SELECT DBSysJobHistory.job_id
									,DBSysJobHistory.step_id
									,MAX(DBSysJobHistory.instance_id) as instance_id
									FROM msdb.dbo.sysjobhistory DBSysJobHistory
									GROUP BY DBSysJobHistory.job_id
									,DBSysJobHistory.step_id
									) AS Instance ON DBSysJobHistory.instance_id = Instance.instance_id
								WHERE DBSysJobHistory.run_status <> 1
								) AS Job
							JOIN msdb.dbo.sysjobs SysJobs
							   ON (Job.job_id = SysJobs.job_id)
							JOIN msdb.dbo.sysjobsteps SysJobSteps
							   ON (Job.job_id = SysJobSteps.job_id AND Job.step_id = SysJobSteps.step_id) ";


		/// <summary>
		/// 服务账户报告
		/// </summary>
		public static string _sqlstrService_Account_Information = @"SELECT @@SERVERNAME ServerName,* FROM sys.dm_server_registry";
		
		/// <summary>
		/// 数据库外键报告
		/// </summary>
		public static string _sqlstrFK_WT_Index = @"if OBJECT_ID('tempdb..#tmp_databases') is not null
						drop table #tmp_databases;
						if OBJECT_ID('tempdb..#Ind') is not null
						drop table #Ind; 
						CREATE TAble #tmp_databases
						(
						id int identity(1,1),
						dbname varchar(200)
						);
						create table #Ind
						(
						databasename	nvarchar(256),
						SchemaName	nvarchar(256),
						TableName	nvarchar(256),
						ConstraintName	nvarchar(256),
						ReferencedSchemaName	nvarchar(256),
						ReferencedTableName	nvarchar(256)
						);

						insert into #tmp_databases
						select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

						DECLARE @min int=1,@max int =0
						select @max=count(*) from #tmp_databases

						while(@min <=@max)
						begin
						DECLARE @SQL VARCHAR(8000) = ''
						,@dbname varchar(200) = ''

						SELECT @dbname=dbname from #tmp_databases where id = @min


							SELECT @SQL=' USE ['+@dbname+']	
								 ; WITH FKTable 
									as(
										SELECT schema_name(o.schema_id) AS ''parent_schema_name'',object_name(FKC.parent_object_id) ''parent_table_name'',
										object_name(constraint_object_id) AS ''constraint_name'',schema_name(RO.Schema_id) AS ''referenced_schema'',object_name(referenced_object_id) AS ''referenced_table_name'',
										(SELECT ''[''+col_name(k.parent_object_id,parent_column_id) +'']'' AS [data()]
										  FROM sys.foreign_key_columns (NOLOCK) AS k
										  INNER JOIN sys.foreign_keys (NOLOCK)
										  ON k.constraint_object_id =object_id
										  AND k.constraint_object_id =FKC.constraint_object_id
										  ORDER BY constraint_column_id
										  FOR XML PATH('''') 
										) AS ''parent_colums'',
										(SELECT ''[''+col_name(k.referenced_object_id,referenced_column_id) +'']'' AS [data()]
										  FROM sys.foreign_key_columns (NOLOCK) AS k
										  INNER JOIN sys.foreign_keys (NOLOCK)
										  ON k.constraint_object_id =object_id
										  AND k.constraint_object_id =FKC.constraint_object_id
										  ORDER BY constraint_column_id
										  FOR XML PATH('''') 
										) AS ''referenced_columns''
									  FROM sys.foreign_key_columns FKC (NOLOCK)
									  INNER JOIN sys.objects o (NOLOCK) ON FKC.parent_object_id = o.object_id
									  INNER JOIN sys.objects RO (NOLOCK) ON FKC.referenced_object_id = RO.object_id
									  WHERE o.object_id in (SELECT object_id FROM sys.objects (NOLOCK) WHERE type =''U'') AND RO.object_id in (SELECT object_id FROM sys.objects (NOLOCK) WHERE type =''U'')
									  group by o.schema_id,RO.schema_id,FKC.parent_object_id,constraint_object_id,referenced_object_id
									),
									/* Index Columns */
									IndexColumnsTable AS
									(
									  SELECT distinct schema_name (o.schema_id) AS ''schema_name'',object_name(o.object_id) AS TableName,
									  (SELECT case key_ordinal when 0 then NULL else ''[''+col_name(k.object_id,column_id) +'']'' end AS [data()]
										FROM sys.index_columns (NOLOCK) AS k
										WHERE k.object_id = i.object_id
										AND k.index_id = i.index_id
										ORDER BY key_ordinal, column_id
										FOR XML PATH('''')
									  ) AS cols
									  FROM sys.indexes (NOLOCK) AS i
									  INNER JOIN sys.objects o (NOLOCK) ON i.object_id =o.object_id 
									  INNER JOIN sys.index_columns ic (NOLOCK) ON ic.object_id =i.object_id AND ic.index_id =i.index_id
									  INNER JOIN sys.columns c (NOLOCK) ON c.object_id = ic.object_id AND c.column_id = ic.column_id
									  WHERE i.object_id in (SELECT object_id FROM sys.objects (NOLOCK) WHERE type =''U'') AND i.index_id > 0
									  group by o.schema_id,o.object_id,i.object_id,i.Name,i.index_id,i.type
									)
									INSERT INTO #Ind
									 SELECT 
									  db_name(),
									  fk.parent_schema_name AS SchemaName,
									  fk.parent_table_name AS TableName,
									  fk.constraint_name AS ConstraintName,
									  fk.referenced_schema AS ReferencedSchemaName,
									  fk.referenced_table_name AS ReferencedTableName
									FROM FKTable fk 
									WHERE (SELECT COUNT(*) AS NbIndexes  FROM IndexColumnsTable ict  WHERE fk.parent_schema_name = ict.schema_name AND fk.parent_table_name = ict.TableName      AND fk.parent_colums = ict.cols
									  ) = 0
									  '

							EXEC (@SQL) 
							set @min = @min+1
						end		
						select @@SERVERNAME AS ServerName,* from #Ind
						DROP TABLE #Ind
						DROP TABLE #tmp_databases ";



		/// <summary>
		/// 冗余索引
		/// </summary>
		public static string _sqlstrRedundant_Indexes = @"
							if OBJECT_ID('tempdb..#tmp_databases') is not null
							drop table #tmp_databases;
							if OBJECT_ID('tempdb..#Ind') is not null
							drop table #Ind; 
							CREATE TAble #tmp_databases
							(
								id int identity(1,1),
								dbname varchar(200)
							);
							create table #Ind
							(
								databasename	nvarchar(256),
								SchemaName		nvarchar(256),
								TableName		nvarchar(256),
								IndexName		nvarchar(256),
								IndexCols		nvarchar(2000),
								RedundantIndexName	nvarchar(256),
								RedundantIndexCols	nvarchar(2000),
								object_id			int,
								index_id			int	 
							);

							insert into #tmp_databases
							select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

							DECLARE @min int=1,@max int =0
							select @max=count(*) from #tmp_databases

							while(@min <=@max)
							begin
								DECLARE @SQL VARCHAR(8000) = ''
								,@dbname varchar(200) = ''

								SELECT @dbname=dbname from #tmp_databases where id = @min


									SELECT @SQL=' USE ['+@dbname+']
										  ;with IndexColumns AS(
											select distinct  schema_name (o.schema_id) as ''SchemaName'',object_name(o.object_id) as TableName, i.Name as IndexName, o.object_id,i.index_id,i.type,
											(select case key_ordinal when 0 then NULL else ''[''+col_name(k.object_id,column_id) +'']'' end as [data()]
											from sys.index_columns  (NOLOCK) as k
											where k.object_id = i.object_id
											and k.index_id = i.index_id
											order by key_ordinal, column_id
											for xml path('''')) as cols,
											(select case key_ordinal when 0 then NULL else ''[''+col_name(k.object_id,column_id) +''] '' + CASE WHEN is_descending_key=1 THEN ''Desc'' ELSE ''Asc'' END end as [data()]
											from sys.index_columns  (NOLOCK) as k
											where k.object_id = i.object_id
											and k.index_id = i.index_id
											order by key_ordinal, column_id
											for xml path('''')) as colsWithSortOrder,
											case when i.index_id=1 then 
											(select ''[''+name+'']'' as [data()]
											from sys.columns  (NOLOCK) as c
											where c.object_id = i.object_id
											and c.column_id not in (select column_id from sys.index_columns  (NOLOCK) as kk    where kk.object_id = i.object_id and kk.index_id = i.index_id)
											order by column_id for xml path(''''))
											else
											(select ''[''+col_name(k.object_id,column_id) +'']'' as [data()]
											from sys.index_columns  (NOLOCK) as k
											where k.object_id = i.object_id
											and k.index_id = i.index_id and is_included_column=1 and k.column_id not in (Select column_id from sys.index_columns kk where k.object_id=kk.object_id and kk.index_id=1)
											order by key_ordinal, column_id for xml path('''')) end as inc
											from sys.indexes  (NOLOCK) as i
											inner join sys.objects o  (NOLOCK) on i.object_id =o.object_id 
											inner join sys.index_columns ic  (NOLOCK) on ic.object_id =i.object_id and ic.index_id =i.index_id
											inner join sys.columns c  (NOLOCK) on c.object_id = ic.object_id and c.column_id = ic.column_id
											where  o.type = ''U'' and i.index_id <>0 and i.type <>3 and i.type <>5 and i.type <>6 and i.type <>7
											group by o.schema_id,o.object_id,i.object_id,i.Name,i.index_id,i.type
											), ResultTable AS
											(SELECT    ic1.SchemaName,ic1.TableName,ic1.IndexName,ic1.object_id, ic2.IndexName as RedundantIndexName, CASE WHEN ic1.index_id=1 THEN ic1.colsWithSortOrder + '' (Clustered)'' WHEN ic1.inc = '''' THEN ic1.colsWithSortOrder  WHEN ic1.inc is NULL THEN ic1.colsWithSortOrder ELSE ic1.colsWithSortOrder + '' INCLUDE '' + ic1.inc END as IndexCols, 
											CASE WHEN ic2.index_id=1 THEN ic2.colsWithSortOrder + '' (Clustered)'' WHEN ic2.inc = '''' THEN ic2.colsWithSortOrder  WHEN ic2.inc is NULL THEN ic2.colsWithSortOrder ELSE ic2.colsWithSortOrder + '' INCLUDE '' + ic2.inc END as RedundantIndexCols, ic1.index_id
											,ic1.cols col1,ic2.cols col2
											from IndexColumns ic1 join IndexColumns ic2 on ic1.object_id = ic2.object_id
											and ic1.index_id <> ic2.index_id and not (ic1.colsWithSortOrder = ic2.colsWithSortOrder and ISNULL(ic1.inc,'''') = ISNULL(ic2.inc,''''))
											and not (ic1.index_id=1 AND ic1.cols = ic2.cols ) and ic1.cols like REPLACE (ic2.cols , ''['',''[[]'') + ''%''
											)
											INSERT INTO #Ind
											SELECT DB_NAME(),SchemaName,TableName, IndexName, IndexCols, RedundantIndexName, RedundantIndexCols, object_id, index_id
											FROM ResultTable
											ORDER BY 1,2,3,5
											  '
									--print @SQL
									EXEC (@SQL) 
									set @min = @min+1
								end		
								select @@SERVERNAME AS ServerName,* from #Ind
								DROP TABLE #Ind
								DROP TABLE #tmp_databases
													";

		/// <summary>
		/// Duplicate_Indexes重复索引
		/// </summary>
		public static string _sqlstrDuplicate_Indexes = @"if OBJECT_ID('tempdb..#tmp_databases') is not null
						drop table #tmp_databases;
						if OBJECT_ID('tempdb..#Ind') is not null
						drop table #Ind; 
						CREATE TAble #tmp_databases
						(
							id int identity(1,1),
							dbname varchar(200)
						);
						create table #Ind 
						(
							databasename	nvarchar(256),
							SchemaName		nvarchar(256),
							TableName		nvarchar(256),
							IndexName		nvarchar(256),
							DuplicateIndexName	nvarchar(256),
							IndexCols		nvarchar(2000), 
							index_id			int	,
							[object_id]			int

						);

						insert into #tmp_databases
						select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

						DECLARE @min int=1,@max int =0
						select @max=count(*) from #tmp_databases

						while(@min <=@max)
						begin
							DECLARE @SQL VARCHAR(8000) = ''
							,@dbname varchar(200) = ''

							SELECT @dbname=dbname from #tmp_databases where id = @min


								SELECT @SQL=' USE ['+@dbname+']	
										;with IndexColumns AS(
										select distinct  schema_name (o.schema_id) as ''SchemaName'',object_name(o.object_id) as TableName, i.Name as IndexName, o.object_id,i.index_id,i.type,
										(select case key_ordinal when 0 then NULL else ''[''+col_name(k.object_id,column_id) +''] '' + CASE WHEN is_descending_key=1 THEN ''Desc'' ELSE ''Asc'' END end as [data()]
										from sys.index_columns  (NOLOCK) as k
										where k.object_id = i.object_id
										and k.index_id = i.index_id
										order by key_ordinal, column_id
										for xml path('''')) as cols,
										case when i.index_id=1 then 
										(select ''[''+name+'']'' as [data()]
										from sys.columns  (NOLOCK) as c
										where c.object_id = i.object_id
										and c.column_id not in (select column_id from sys.index_columns  (NOLOCK) as kk    where kk.object_id = i.object_id and kk.index_id = i.index_id)
										order by column_id
										for xml path(''''))
										else (select ''[''+col_name(k.object_id,column_id) +'']'' as [data()]
										from sys.index_columns  (NOLOCK) as k
										where k.object_id = i.object_id
										and k.index_id = i.index_id and is_included_column=1 and k.column_id not in (Select column_id from sys.index_columns kk where k.object_id=kk.object_id and kk.index_id=1)
										order by key_ordinal, column_id
										for xml path('''')) end as inc
										from sys.indexes  (NOLOCK) as i
										inner join sys.objects o  (NOLOCK) on i.object_id =o.object_id 
										inner join sys.index_columns ic  (NOLOCK) on ic.object_id =i.object_id and ic.index_id =i.index_id
										inner join sys.columns c  (NOLOCK) on c.object_id = ic.object_id and c.column_id = ic.column_id
										where  o.type = ''U'' and i.index_id <>0 and i.type <>3 and i.type <>5 and i.type <>6 and i.type <>7 
										group by o.schema_id,o.object_id,i.object_id,i.Name,i.index_id,i.type
										),
										DuplicatesTable AS
										(SELECT    ic1.SchemaName,ic1.TableName,ic1.IndexName,ic1.object_id, ic2.IndexName as DuplicateIndexName, 
										CASE WHEN ic1.index_id=1 THEN ic1.cols + '' (Clustered)'' WHEN ic1.inc = '''' THEN ic1.cols  WHEN ic1.inc is NULL THEN ic1.cols ELSE ic1.cols + '' INCLUDE '' + ic1.inc END as IndexCols, 
										ic1.index_id
										from IndexColumns ic1 join IndexColumns ic2 on ic1.object_id = ic2.object_id
										and ic1.index_id < ic2.index_id and ic1.cols = ic2.cols
										and (ISNULL(ic1.inc,'''') = ISNULL(ic2.inc,'''')  OR ic1.index_id=1 )
										)
										INSERT INTO #Ind
										SELECT DB_NAME(),SchemaName,TableName, IndexName,DuplicateIndexName, IndexCols, index_id, object_id
										FROM DuplicatesTable dt
										ORDER BY 1,2,3
										  '
								--print @SQL
								EXEC (@SQL) 
								set @min = @min+1
						end		
							select @@SERVERNAME AS ServerName,* from #Ind
						DROP TABLE #Ind
						DROP TABLE #tmp_databases";





		/// <summary>
		/// 数据库列信息
		/// </summary>
		public static string _sqlstrTable_Columns_Info = @"
							if OBJECT_ID('tempdb..#tmp_databases') is not null
								drop table #tmp_databases;
								if OBJECT_ID('tempdb..#Ind') is not null
								drop table #Ind; 
								CREATE TAble #tmp_databases
								(
									id int identity(1,1),
									dbname varchar(200)
								);
								create table #Ind 
								(
								dbname					nvarchar(256),
								TABLE_SCHEMA				nvarchar(256),
								TABLE_NAME				nvarchar(256),
								COLUMN_NAME				nvarchar(256),
								ORDINAL_POSITION			int,
								[COLUMN_DEFAULT]			varchar(8000),
								IS_NULLABLE				varchar(3),
								DATA_TYPE				nvarchar(256),
								CHARACTER_MAXIMUM_LENGTH		int,
								CHARACTER_OCTET_LENGTH			int,
								NUMERIC_PRECISION			tinyint,
								NUMERIC_PRECISION_RADIX			smallint,
								NUMERIC_SCALE				int,
								DATETIME_PRECISION			smallint,
								CHARACTER_SET_CATALOG			nvarchar(256),
								CHARACTER_SET_SCHEMA			nvarchar(256),
								CHARACTER_SET_NAME			nvarchar(256),
								COLLATION_CATALOG			nvarchar(256),
								COLLATION_SCHEMA			nvarchar(256),
								COLLATION_NAME				nvarchar(256),
								DOMAIN_CATALOG				nvarchar(256),
								DOMAIN_SCHEMA				nvarchar(256),
								DOMAIN_NAME				nvarchar(256)
							) 

							insert into #tmp_databases
							select name from sys.databases 
							where is_read_only=0 and state_desc='ONLINE' 
							and name not in ('master','model','tempdb','msdb');

							DECLARE @min int=1,@max int =0
							select @max=count(*) from #tmp_databases

							while(@min <=@max)
							begin
								DECLARE @SQL VARCHAR(8000) = ''
								,@dbname varchar(200) = ''

								SELECT @dbname=dbname from #tmp_databases where id = @min


									SELECT @SQL=' USE ['+@dbname+'];	
										INSERT INTO #Ind
										select  
											 TABLE_CATALOG	 AS DBNAME			
											,TABLE_SCHEMA				
											,TABLE_NAME					
											,COLUMN_NAME					
											,ORDINAL_POSITION			
											,[COLUMN_DEFAULT]			
											,IS_NULLABLE					
											,DATA_TYPE					
											,CHARACTER_MAXIMUM_LENGTH	
											,CHARACTER_OCTET_LENGTH		
											,NUMERIC_PRECISION			
											,NUMERIC_PRECISION_RADIX		
											,NUMERIC_SCALE				
											,DATETIME_PRECISION			
											,CHARACTER_SET_CATALOG		
											,CHARACTER_SET_SCHEMA		
											,CHARACTER_SET_NAME			
											,COLLATION_CATALOG			
											,COLLATION_SCHEMA			
											,COLLATION_NAME				
											,DOMAIN_CATALOG				
											,DOMAIN_SCHEMA				
											,DOMAIN_NAME	 
										from INFORMATION_SCHEMA.COLUMNS
										WHERE TABLE_CATALOG=DB_NAME()
										'
							--print @SQL
										EXEC (@SQL) 
										set @min = @min+1
								end		
							 select @@SERVERNAME AS ServerName,* from #Ind
								DROP TABLE #Ind
								DROP TABLE #tmp_databases";








		/// <summary>
		/// 无索引表
		/// </summary>
		public static string _sqlstrTable_WT_Indexes = @"if OBJECT_ID('tempdb..#tmp_databases') is not null
								drop table #tmp_databases;
								if OBJECT_ID('tempdb..#Ind') is not null
								drop table #Ind; 
								CREATE TAble #tmp_databases
								(
								id int identity(1,1),
								dbname varchar(200)
								);
								create table #Ind 
								(
								databasename	nvarchar(256),
								SchemaName		nvarchar(256),
								TableName		nvarchar(256), 
								[object_id]			int,
								ApproximateRows		bigint,
								ColumnCount			int

								);

								insert into #tmp_databases
								select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

								DECLARE @min int=1,@max int =0
								select @max=count(*) from #tmp_databases

								while(@min <=@max)
								begin
								DECLARE @SQL VARCHAR(8000) = ''
								,@dbname varchar(200) = ''

								SELECT @dbname=dbname from #tmp_databases where id = @min


									SELECT @SQL=' USE ['+@dbname+'];	
											INSERT INTO #Ind
											SELECT DISTINCT 
												db_name(),
												schema_name(so.schema_id) AS [SchemaName], 
												object_name(so.object_id) AS [TableName],
												so.object_id		AS [object_id],
												max(dmv.rows)		AS [ApproximateRows],
												MAX(d.ColumnCount)	AS [ColumnCount]
											FROM sys.objects so (NOLOCK) 
											JOIN sys.indexes si (NOLOCK) ON so.object_id = si.object_id AND so.type in (N''U'',N''V'') 
											JOIN sysindexes dmv (NOLOCK) ON so.object_id = dmv.id AND si.index_id = dmv.indid
											FULL OUTER JOIN (SELECT object_id, count(1) AS ColumnCount FROM sys.columns (NOLOCK) GROUP BY object_id) d ON d.object_id = so.object_id
											WHERE so.is_ms_shipped = 0 AND so.object_id NOT IN (SELECT major_id FROM sys.extended_properties (NOLOCK) WHERE name = N''microsoft_database_tools_support'') 
											AND indexproperty(so.object_id, si.name, ''IsStatistics'') = 0
											GROUP BY so.schema_id, so.object_id
											HAVING( CASE objectproperty(MAX(so.object_id), ''TableHasClustIndex'') WHEN 0 THEN COUNT(si.index_id) - 1 ELSE COUNT(si.index_id) END  = 0)
											ORDER BY SchemaName, TableName;
										 '
									--print @SQL
									EXEC (@SQL) 
									set @min = @min+1
								end		
								select @@SERVERNAME AS ServerName,* from #Ind
								DROP TABLE #Ind
								DROP TABLE #tmp_databases";


		/// <summary>
		/// 无聚集索引表
		/// </summary>
		public static string _sqlstrTable_WT_CL_Indexes = @"if OBJECT_ID('tempdb..#tmp_databases') is not null
									drop table #tmp_databases;
									if OBJECT_ID('tempdb..#Ind') is not null
									drop table #Ind; 
									CREATE TAble #tmp_databases
									(
									id int identity(1,1),
									dbname varchar(200)
									);
									create table #Ind 
									(
										databasename	nvarchar(256),
										SchemaName		nvarchar(256),
										TableName		nvarchar(256), 
										[object_id]			int,
										ApproximateRows		bigint,
										IndexCount			int,
										ColumnCount			int,
										Indexsize_MB		BIGINT
									);

									insert into #tmp_databases
									select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

									DECLARE @min int=1,@max int =0
									select @max=count(*) from #tmp_databases

									while(@min <=@max)
									begin
									DECLARE @SQL VARCHAR(8000) = ''
									,@dbname varchar(200) = ''

									SELECT @dbname=dbname from #tmp_databases where id = @min


										SELECT @SQL=' USE ['+@dbname+'];	
												INSERT INTO #Ind
												SELECT DISTINCT 
													db_name(), 
													schema_name(so.schema_id) AS [SchemaName],
													object_name(so.object_id) AS [TableName],
													so.object_id AS [object_id], 
													max(dmv.rows) AS [ApproximateRows], 
													CASE objectproperty(MAX(so.object_id), ''TableHasClustIndex'') 
													WHEN 0 THEN count(si.index_id) - 1 ELSE COUNT(si.index_id) END as [IndexCount], 
													MAX(d.ColumnCount) AS [ColumnCount],
													MAX(indexspace.Indexsize_MB)
												FROM sys.objects so (NOLOCK)
												JOIN sys.indexes si (NOLOCK) ON so.object_id = si.object_id AND so.type in (N''U'',N''V'') 
												JOIN sysindexes dmv (NOLOCK) ON so.object_id = dmv.id AND si.index_id = dmv.indid
												FULL OUTER JOIN (SELECT object_id, count(1) AS ColumnCount FROM sys.columns (NOLOCK) GROUP BY object_id) d  ON d.object_id = so.object_id
												LEFT JOIN
												(
														SELECT  tn.object_id as object_id,ss.schema_id,
														(SUM(sz.[used_page_count]) * 8.0)/1024.0 AS [Indexsize_MB]
														FROM sys.dm_db_partition_stats AS sz
														INNER JOIN sys.indexes AS ix ON sz.[object_id] = ix.[object_id] AND sz.[index_id] = ix.[index_id]
														INNER JOIN sys.tables tn ON tn.OBJECT_ID = ix.object_id and tn.type =''U''
														INNER JOIN sys.schemas ss ON ss.schema_id = tn.schema_id
														GROUP BY tn.object_id, ss.schema_id
												)indexspace ON  indexspace.schema_id=so.schema_id and so.object_id=indexspace.object_id 
												WHERE so.is_ms_shipped = 0
												AND so.object_id NOT IN (SELECT major_id FROM sys.extended_properties (NOLOCK) WHERE name = N''microsoft_database_tools_support'')
												AND indexproperty(so.object_id, si.name, ''IsStatistics'') = 0
												GROUP BY so.schema_id, so.object_id,indexspace.Indexsize_MB
												HAVING (objectproperty(max(so.object_id), ''TableHasClustIndex'') = 0 
												--AND COUNT(si.index_id)-1 > 0
												)
				

												'
										--print @SQL
										EXEC (@SQL) 
										set @min = @min+1
									end		
									select @@SERVERNAME AS ServerName,d.*  
									from #Ind d
									DROP TABLE #Ind
									DROP TABLE #tmp_databases 
										";
		/// <summary>
		/// 列总计超过900Bytes索引
		/// </summary>
		public static string _sqlstrIndex_GT_900_Bytes = @" if OBJECT_ID('tempdb..#tmp_databases') is not null
								drop table #tmp_databases;
								if OBJECT_ID('tempdb..#Ind') is not null
								drop table #Ind; 
								CREATE TAble #tmp_databases
								(
								id int identity(1,1),
								dbname varchar(200)
								);
								create table #Ind 
								(
								databasename	nvarchar(256),
								SchemaName		nvarchar(256),
								TableName		nvarchar(256), 
								IndexName		nvarchar(256), 
								IndexType		nvarchar(256), 
								RowLength			INT,
								ColumnCount			int

								);

								insert into #tmp_databases
								select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

								DECLARE @min int=1,@max int =0
								select @max=count(*) from #tmp_databases

								while(@min <=@max)
								begin
								DECLARE @SQL VARCHAR(8000) = ''
								,@dbname varchar(200) = ''

								SELECT @dbname=dbname from #tmp_databases where id = @min


									SELECT @SQL=' USE ['+@dbname+'];	
											INSERT INTO #Ind
											SELECT DISTINCT 
												db_name(), 
												schema_name (o.schema_id) AS [SchemaName],
												o.name AS TableName, 
												i.name AS IndexName, 
												i.type_desc AS IndexType, 
												sum(max_length) AS RowLength, 
												count (ic.index_id) AS [ColumnCount]
											FROM sys.indexes i (NOLOCK) 
											INNER JOIN sys.objects o (NOLOCK)  ON i.object_id =o.object_id 
											INNER JOIN sys.index_columns ic  (NOLOCK) ON ic.object_id =i.object_id and ic.index_id =i.index_id
											INNER JOIN sys.columns c  (NOLOCK) ON c.object_id = ic.object_id and c.column_id = ic.column_id
											WHERE o.type =''U'' and i.index_id >0 and ic.is_included_column=0
											GROUP BY o.schema_id,o.object_id,o.name,i.object_id,i.name,i.index_id,i.type_desc
											HAVING (sum(max_length) > 900)
											ORDER BY 1,2,3
											'
									--print @SQL
									EXEC (@SQL) 
									set @min = @min+1
								end		
								select @@SERVERNAME AS ServerName,* from #Ind
								DROP TABLE #Ind
								DROP TABLE #tmp_databases";


		/// <summary>
		/// 阻止的会话
		/// </summary>
		public static string _sqlstrBlocking_Sessions = @"SELECT @@SERVERNAME AS ServerName,t1.resource_type AS [lock type], DB_NAME(resource_database_id) AS [database],
							t1.resource_associated_entity_id AS [blk object],t1.request_mode AS [lock req],  -- lock requested
							t1.request_session_id AS [waiter sid], t2.wait_duration_ms AS [wait time],       -- spid of waiter  
							(SELECT [text] FROM sys.dm_exec_requests AS r WITH (NOLOCK)                      -- get sql for waiter
							CROSS APPLY sys.dm_exec_sql_text(r.[sql_handle]) 
							WHERE r.session_id = t1.request_session_id) AS [waiter_batch],
							(SELECT SUBSTRING(qt.[text],r.statement_start_offset/2, 
							(CASE WHEN r.statement_end_offset = -1 
							THEN LEN(CONVERT(nvarchar(max), qt.[text])) * 2 
							ELSE r.statement_end_offset END - r.statement_start_offset)/2) 
							FROM sys.dm_exec_requests AS r WITH (NOLOCK)
							CROSS APPLY sys.dm_exec_sql_text(r.[sql_handle]) AS qt
							WHERE r.session_id = t1.request_session_id) AS [waiter_stmt],					-- statement blocked
							t2.blocking_session_id AS [blocker sid],										-- spid of blocker
							(SELECT [text] FROM sys.sysprocesses AS p										-- get sql for blocker
							CROSS APPLY sys.dm_exec_sql_text(p.[sql_handle]) 
							WHERE p.spid = t2.blocking_session_id) AS [blocker_batch]
							FROM sys.dm_tran_locks AS t1 WITH (NOLOCK)
							INNER JOIN sys.dm_os_waiting_tasks AS t2 WITH (NOLOCK)
							ON t1.lock_owner_address = t2.resource_address OPTION (RECOMPILE);";
	
		
		/// <summary>
		/// 代价过高的功能
		/// </summary>
		 public static string _sqlstrExpensive_Functions = @"SET NOCOUNT ON
						DECLARE @ProductVersion NVARCHAR(128)
						SET @ProductVersion = CAST(SERVERPROPERTY ('ProductVersion') AS NVARCHAR(128))

						IF CAST(LEFT(@ProductVersion, CHARINDEX('.',@ProductVersion)-1) AS INT) >= 13
						BEGIN
								if OBJECT_ID('tempdb..#tmp_databases') is not null
								drop table #tmp_databases;
								if OBJECT_ID('tempdb..#function_stats') is not null
								drop table #function_stats; 
								CREATE TAble #tmp_databases
								(
									id int identity(1,1),
									dbname varchar(200)
								);
								create table #function_stats 
								(
								databasename	 nvarchar(256),
								FunctionName	 nvarchar(256), 
								execution_count	 BIGINT,
								total_worker_time BIGINT,
								total_logical_reads BIGINT,
								total_physical_reads BIGINT,
								total_elapsed_time BIGINT,
								[avg_elapsed_time] BIGINT,
								 [Plan_Cached_Time] DATETIME

								);

								insert into #tmp_databases
								select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

								DECLARE @min int=1,@max int =0
								select @max=count(*) from #tmp_databases

								while(@min <=@max)
								begin
								DECLARE @SQL VARCHAR(8000) = ''
								,@dbname varchar(200) = ''

								SELECT @dbname=dbname from #tmp_databases where id = @min


									SELECT @SQL=' USE ['+@dbname+'];	
											INSERT INTO #function_stats
											SELECT DISTINCT 
												db_name(), 
												OBJECT_NAME(object_id) AS [Function Name], 
												execution_count,
												total_worker_time, 
												total_logical_reads, 
												total_physical_reads, 
												total_elapsed_time, 
												total_elapsed_time/execution_count AS [avg_elapsed_time],
												FORMAT(cached_time, ''yyyy-MM-dd HH:mm:ss'', ''en-US'') AS [Plan Cached Time]
											FROM sys.dm_exec_function_stats WITH (NOLOCK) 
											WHERE database_id = DB_ID()
											ORDER BY total_worker_time DESC OPTION (RECOMPILE);
											'
									--print @SQL
									EXEC (@SQL) 
									set @min = @min+1
								end	;
								select @@SERVERNAME AS ServerName,* from #function_stats
								DROP TABLE #function_stats
								DROP TABLE #tmp_databases
						end 
								";


		/// <summary>
		/// 日志空间使用
		/// </summary>
		public static string _sqlstrLog_Space_Usage = @"	SET NOCOUNT ON
							DECLARE @ProductVersion NVARCHAR(128)
							SET @ProductVersion = CAST(SERVERPROPERTY ('ProductVersion') AS NVARCHAR(128))

							IF CAST(LEFT(@ProductVersion, CHARINDEX('.',@ProductVersion)-1) AS INT) >= 13
							BEGIN
								if OBJECT_ID('tempdb..#tmp_databases') is not null
								drop table #tmp_databases;
								if OBJECT_ID('tempdb..#Log_space') is not null
								drop table #Log_space; 
								CREATE TAble #tmp_databases
								(
									id int identity(1,1),
									dbname varchar(200)
								);
								create table #Log_space 
								(
									databasename	 nvarchar(256),
									RecoveryModel	 nvarchar(256), 
									[Total_Log_Space_(MB)] DECIMAL(10, 2),
									[Used_Log_Space_(MB)] DECIMAL(10, 2) ,
									[Used_Log_Space_%] DECIMAL(10, 2),
									[Used_Log_Space_Since_Last_Backup_(MB)] DECIMAL(10, 2),
									log_reuse_wait_desc  nvarchar(512)
								);

								insert into #tmp_databases
								select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

								DECLARE @min int=1,@max int =0
								select @max=count(*) from #tmp_databases

								while(@min <=@max)
								begin
								DECLARE @SQL VARCHAR(8000) = ''
								,@dbname varchar(200) = ''

								SELECT @dbname=dbname from #tmp_databases where id = @min


									SELECT @SQL=' USE ['+@dbname+'];	
											INSERT INTO #Log_space
											SELECT   
													DB_NAME(lsu.database_id) AS [Database Name], 
													db.recovery_model_desc AS [RecoveryModel],
													CAST(lsu.total_log_size_in_bytes/1048576.0 AS DECIMAL(10, 2)) AS [Total_Log_Space (MB)],
													CAST(lsu.used_log_space_in_bytes/1048576.0 AS DECIMAL(10, 2)) AS [Used Log Space (MB)], 
													CAST(lsu.used_log_space_in_percent AS DECIMAL(10, 2)) AS [Used Log Space %],
													CAST(lsu.log_space_in_bytes_since_last_backup/1048576.0 AS DECIMAL(10, 2)) AS [Used Log Space Since Last Backup (MB)],
													db.log_reuse_wait_desc		 
											FROM sys.dm_db_log_space_usage AS lsu WITH (NOLOCK)
											INNER JOIN sys.databases AS db WITH (NOLOCK)
											ON lsu.database_id = db.database_id
											OPTION (RECOMPILE);	
											'
									--print @SQL
									EXEC (@SQL) 
									set @min = @min+1
								end		
								select @@SERVERNAME AS ServerName,* from #Log_space
								DROP TABLE #Log_space
								DROP TABLE #tmp_databases
							end
						";
		/// <summary>
		/// 环形缓冲区内存使用报告
		/// </summary>
		public static string _sqlstrRing_Buffer_Memory_Usage = @";with table1 as(
												SELECT  CONVERT (varchar(30), GETDATE(), 121) as runtime, DATEADD (ms, a.[Record Time] - sys.ms_ticks, GETDATE()) AS Notification_time,    a.*  
												FROM   (SELECT x.value('(//Record/SchedulerMonitorEvent/SystemHealth/ProcessUtilization)[1]', 'int') AS [ProcessUtilization],
												x.value('(//Record/SchedulerMonitorEvent/SystemHealth/WorkingSetDelta) [1]', 'bigint')/1024 AS [WorkingSetDelta],
												x.value('(//Record/SchedulerMonitorEvent/SystemHealth/MemoryUtilization) [1]', 'bigint') AS [MemoryUtilization],
												x.value('(//Record/@time)[1]', 'bigint') AS [Record Time]  FROM (SELECT CAST (record as xml) FROM sys.dm_os_ring_buffers
												WHERE ring_buffer_type = 'RING_BUFFER_SCHEDULER_MONITOR') AS R(x)) a 
												CROSS JOIN sys.dm_os_sys_info sys 
												)
												select @@SERVERNAME AS ServerName,*  
												from table1";


		/// <summary>
		/// 数据库镜像信息
		/// </summary>
		public static string _sqlstrDB_Mirroring_Info = @"SELECT
							SERVERPROPERTY('ServerName') AS Principal,
							m.mirroring_partner_instance AS DR, 
							m.mirroring_state_desc, 
							m.mirroring_role_desc,   
							m.mirroring_partner_name, 
							m.mirroring_witness_name,
							m.mirroring_witness_state_desc,
							DB_NAME(m.database_id) AS[Database],
							CASE m.mirroring_safety_level_desc WHEN 'OFF'   THEN 'High Performance' ELSE 'High Safety' END AS [OperatingMode],
							CAST((pc.cntr_value)/1024/1024 AS DECIMAL(10,3)) AS unsentGB
						FROM sys.database_mirroring m
						JOIN sys.dm_os_performance_counters pc ON DB_NAME(m.database_id) = pc.instance_name
						WHERE m.mirroring_state IS NOT NULL
						AND m.mirroring_state<> 4
						AND pc.object_name LIKE '%Database Mirroring%'
						AND pc.counter_name = 'Log Send Queue KB'";



		/// <summary>
		/// 不受信任的约束
		/// </summary>
		public static string _sqlstrUntrusted_Constraints = @"
					if OBJECT_ID('tempdb..#tmp_databases') is not null
					drop table #tmp_databases;
					if OBJECT_ID('tempdb..#untrustedConstraints') is not null
					drop table #untrustedConstraints; 

					CREATE TAble #tmp_databases
					(
						id int identity(1,1),
						dbname varchar(200)
					);
					CREATE TABLE #untrustedConstraints
					(
						DBName varchar(100),
						TableName VARCHAR(200),
						ConstraintName VARCHAR(100),
						EnableConstraintCommand VARCHAR(2000)
					)

					insert into #tmp_databases
					select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

					DECLARE @min int=1,@max int =0
					select @max=count(*) from #tmp_databases

					while(@min <=@max)
					begin
						DECLARE @SQL VARCHAR(8000) = ''
							  ,@dbname varchar(200) = ''

						SELECT @dbname=dbname from #tmp_databases where id = @min


							SELECT @SQL=' USE ['+@dbname+']
								  INSERT INTO #untrustedConstraints
									SELECT 
										db_name(),
										QUOTENAME(SCHEMA_NAME(i.schema_id)) + ''.'' + QUOTENAME(o.name) AS TableName,
										i.name AS ConstraintName,
										''ALTER TABLE '' + QUOTENAME(SCHEMA_NAME(i.schema_id)) + ''.'' + QUOTENAME(o.name) + '' WITH CHECK CHECK CONSTRAINT ['' + i.name + '']'' AS CheckCommand
									FROM  sys.check_constraints AS i
									INNER JOIN sys.objects AS o  ON i.parent_object_id = o.OBJECT_ID 
									WHERE  i.is_not_trusted = 1 
										AND i.is_not_for_replication = 0 
									UNION ALL 
									SELECT	db_name(),
											QUOTENAME(SCHEMA_NAME(i.schema_id)) + ''.'' + QUOTENAME(o.name) AS TableName, 
											i.name AS ConstraintName, 
											''ALTER TABLE '' + QUOTENAME(SCHEMA_NAME(i.schema_id)) + ''.'' + QUOTENAME(o.name) + '' WITH CHECK CHECK CONSTRAINT ['' + i.name + '']'' AS CheckCommand 
									FROM  sys.foreign_keys AS i 
									INNER JOIN sys.objects AS o ON i.parent_object_id = o.OBJECT_ID
									WHERE  i.is_not_trusted = 1
										AND i.is_not_for_replication = 0 
									ORDER BY TableName, ConstraintName 
									  '
							--print @SQL
							EXEC (@SQL) 
							set @min = @min+1
						end		
						select @@SERVERNAME AS ServerName,* from #untrustedConstraints
						DROP TABLE #untrustedConstraints
						DROP TABLE #tmp_databases ";


		/// <summary>
		/// TOP50重编译查询
		/// </summary>
		 public static string _sqlstrTop_50_Recompile_Qry = @"SELECT TOP 50 
						@@SERVERNAME AS ServerName,
						qs.plan_generation_num, 
						qs.execution_count, 
						DB_NAME(st.dbid) AS 'database', 
						qs.last_execution_time,
						qs.last_logical_reads,
						qs.last_logical_writes,
						qs.last_physical_reads,
						qs.last_worker_time,
						qs.total_worker_time,
						Substring(st.text,0,2000) as SQLText  
					FROM sys.dm_exec_query_stats AS qs 
					CROSS APPLY sys.dm_exec_sql_text(sql_handle) AS st 
					ORDER BY plan_generation_num DESC ";


		/// <summary>
		/// ？////？未知
		/// </summary>
		public static string _sqlstrIndex_GT_Columns = @"if OBJECT_ID('tempdb..#tmp_databases') is not null
						drop table #tmp_databases;
						if OBJECT_ID('tempdb..#IndexCount') is not null
						drop table #IndexCount; 

						CREATE TAble #tmp_databases
						(
							id int identity(1,1),
							dbname varchar(200)
						);
						CREATE TABLE #IndexCount
						(
							DBName			varchar(100),
							[SchemaName]    varchar(100),
							TableName		VARCHAR(200),
							IndexCount		INT,
							ColumnCount		INT
						) 

						insert into #tmp_databases
						select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

						DECLARE @min int=1,@max int =0
						select @max=count(*) from #tmp_databases

						while(@min <=@max)
						begin
						DECLARE @SQL VARCHAR(8000) = ''
						,@dbname varchar(200) = ''

						SELECT @dbname=dbname from #tmp_databases where id = @min


							SELECT @SQL=' USE ['+@dbname+'];
									INSERT INTO #IndexCount
									SELECT DISTINCT   
									DB_NAME() as DBName,
									schema_name(so.schema_id) AS Schema_Name, 
									object_name(so.object_id) AS TableName, 
									CASE objectproperty(max(so.object_id), ''TableHasClustIndex'') 
									WHEN 0 THEN count(si.index_id) - 1  ELSE count(si.index_id) END AS ''IndexCount'', 
									MAX(d.ColumnCount) AS ''ColumnCount''  
									FROM sys.objects so (NOLOCK) 
									JOIN sys.indexes si (NOLOCK) ON so.object_id = si.object_id AND so.type in (N''U'',N''V'') 
									JOIN sysindexes dmv (NOLOCK) ON so.object_id = dmv.id AND si.index_id = dmv.indid 
									FULL OUTER JOIN (SELECT object_id, count(1) AS ColumnCount FROM sys.columns (NOLOCK) GROUP BY object_id) d ON d.object_id = so.object_id 
									WHERE so.is_ms_shipped = 0 
									AND so.object_id not in (select major_id FROM sys.extended_properties (NOLOCK) where name = N''microsoft_database_tools_support'') 
									AND indexproperty(so.object_id, si.name, ''IsStatistics'') = 0 
									GROUP BY so.schema_id, so.object_id
									HAVING(CASE objectproperty(MAX(so.object_id), ''TableHasClustIndex'')
									WHEN 0 THEN COUNT(si.index_id) - 1 
									ELSE COUNT(si.index_id) 
									END > MAX(d.ColumnCount))
										'
							--print @SQL
							EXEC (@SQL) 
							set @min = @min+1
						end		
						select @@SERVERNAME AS ServerName,* from #IndexCount
						DROP TABLE #IndexCount
						DROP TABLE #tmp_databases";


		/// <summary>
		/// SPN检查
		/// </summary>
		public static string _sqlstrSPN_Check = @"SELECT @@SERVERNAME AS ServerName,Count(session_id) as SessionCount,auth_scheme 
						FROM sys.dm_exec_connections 
						WHERE  net_transport <> 'Shared Memory'  
						AND client_net_address <> local_net_address  
						GROUP BY auth_scheme
						ORDER BY SessionCount DESC";


		/// <summary>
		///  虚拟日志文件
		/// </summary>
		public static string _sqlstrVLF_Count = @"
					if(convert(char(12),serverproperty('productversion')) > '11.0')
					begin
					SELECT 
					@@SERVERNAME AS ServerName,
					[name] AS 'Database Name',
					COUNT(l.database_id) AS 'VLF Count',
					SUM(CAST(vlf_active AS INT)) AS 'Active VLF',
					COUNT(l.database_id)-SUM(CAST(vlf_active AS INT)) AS 'Inactive VLF',
					SUM(vlf_size_mb) AS 'VLF Size (MB)',
					SUM(vlf_active*vlf_size_mb) AS 'Active VLF Size (MB)',
					SUM(vlf_size_mb)-SUM(vlf_active*vlf_size_mb) AS 'Inactive VLF Size (MB)'
					FROM sys.databases s
					CROSS APPLY sys.dm_db_log_info(s.database_id) l
					GROUP BY [name]
					ORDER BY COUNT(l.database_id) desc
					end";


		/// <summary>
		/// TempDB详细使用情况
		/// </summary>
		public static string _sqlstrTempDB_Usage_Info = @"
					use tempdb;
					SELECT @@SERVERNAME AS ServerName,LEFT(st.text,2000) AS QueryText,r.*,Sessionspace.*, TASKSPACE.*,d.log_reuse_wait,d.log_reuse_wait_desc,t2.* 
					FROM tempdb.sys.dm_exec_sessions AS t2
					LEFT JOIN tempdb.sys.dm_exec_connections AS t1  ON t1.session_id = t2.session_id         
					OUTER APPLY tempdb.sys.dm_exec_sql_text(t1.most_recent_sql_handle) AS st 
					left JOIN sys.databases d on d.database_id = t2.database_id
					LEFT JOIN (
								SELECT 
								   req.session_id
								   , req.start_time
								   , cpu_time 'cpu_time_ms'
								   , object_name(st.objectid,st.dbid) 'ObjectName' 
								   , substring
									  (REPLACE
										(REPLACE
										  (SUBSTRING
											(ST.text
											, (req.statement_start_offset/2) + 1
											, (
											   (CASE statement_end_offset
												  WHEN -1
												  THEN DATALENGTH(ST.text)  
												  ELSE req.statement_end_offset
												  END
													- req.statement_start_offset)/2) + 1)
									   , CHAR(10), ' '), CHAR(13), ' '), 1, 512)  AS statement_text  
								FROM tempdb.sys.dm_exec_requests AS req  
								   CROSS APPLY tempdb.sys.dm_exec_sql_text(req.sql_handle) as ST 
					) r on r.session_id = t2.session_id
					left JOIN 
					(
						SELECT R2.session_id,
						R1.internal_objects_alloc_page_count   + SUM(R2.internal_objects_alloc_page_count) AS session_internal_objects_alloc_page_count,
						R1.internal_objects_dealloc_page_count  + SUM(R2.internal_objects_dealloc_page_count) AS session_internal_objects_dealloc_page_count
						FROM tempdb.sys.dm_db_session_space_usage AS R1 
						LEFT JOIN tempdb.sys.dm_db_task_space_usage AS R2 ON R1.session_id = R2.session_id
						GROUP BY R2.session_id, R1.internal_objects_alloc_page_count, R1.internal_objects_dealloc_page_count 
					)Sessionspace ON t2.session_id = Sessionspace.session_id
					LEFT JOIN
					(	
						SELECT session_id, 
						SUM(internal_objects_alloc_page_count) AS task_internal_objects_alloc_page_count,
						SUM(internal_objects_dealloc_page_count) AS task_internal_objects_dealloc_page_count 
						FROM tempdb.sys.dm_db_task_space_usage 
						GROUP BY session_id 
					)TASKSPACE ON TASKSPACE.session_id = t2.session_id  
					WHERE   Sessionspace.session_internal_objects_alloc_page_count >0 AND t2.session_id <> @@SPID
					";



		/// <summary>
		/// AG群集状态
		/// </summary>
		public static string _sqlstrAG_Cluster_Status = @"select  @@SERVERNAME AS ServerName,* 
						from sys.dm_os_cluster_nodes n
						outer apply
						(
							select * from sys.dm_os_cluster_properties
						)clp
						";

		/// <summary>
		/// AG健康状态
		/// </summary>
		public static string _sqlstrAG_Health_Status = @"
					if(convert(char(12),serverproperty('productversion')) > '12.0')
					begin

					SELECT
											ar.*,
											ag.name AS 'AG Name',
											ag.is_distributed,
											ar.replica_server_name AS 'AG',
											dbs.name AS 'Database',
											ars.role_desc,
											drs.synchronization_health_desc,
											drs.log_send_queue_size,
											drs.log_send_rate,
											drs.redo_queue_size,
											drs.redo_rate,
											drs.suspend_reason_desc,
											drs.last_sent_time,
											drs.last_received_time,
											drs.last_hardened_time,
											drs.last_redone_time,
											drs.last_commit_time,
											drs.secondary_lag_seconds
											,has.*
											,rcl.*
											,HDR.*
							FROM sys.databases dbs
							INNER JOIN sys.dm_hadr_database_replica_states drs  ON dbs.database_id = drs.database_id
							INNER JOIN sys.availability_groups ag  ON drs.group_id = ag.group_id
							INNER JOIN sys.dm_hadr_availability_replica_states ars  ON ars.replica_id = drs.replica_id
							INNER JOIN sys.availability_replicas ar  ON ar.replica_id =  ars.replica_id
							INNER JOIN sys.dm_hadr_availability_replica_cluster_states rcl  ON rcl.replica_server_name =  ar.replica_server_name
							LEFT JOIN sys.dm_hadr_automatic_seeding has on has.ag_remote_replica_id=ar.replica_id and has.ag_id = ag.group_id and dbs.group_database_id=has.ag_db_id
							LEFT JOIN sys.dm_hadr_database_replica_cluster_states HDR ON HDR.replica_id = ar.replica_id 
					end
							";


		/// <summary>
		/// TempDB打开的交易
		/// </summary>
		public static string _sqlstrTempDB_Open_Tran = @"SELECT 
									 [Source] = 'database_transactions'
									,[session_id] = ST.session_id
									,[transaction_id] = ST.transaction_id
									,[database_id] = DT.database_id
									,[database_name] = CASE
										WHEN D.name IS NULL AND DT.database_id = 2 THEN 'TEMPDB'
										ELSE D.name
									 END
									,[database_transaction_log_used_Kb] = CONVERT(numeric(18,2), DT.database_transaction_log_bytes_used / 1024.0 )
									,[database_transaction_begin_time] = DT.database_transaction_begin_time
									,[transaction_type_desc] = CASE database_transaction_type
										WHEN 1 THEN 'Read/write transaction'
										WHEN 2 THEN 'Read-only transaction'
										WHEN 3 THEN 'System transaction'
										WHEN 4 THEN 'Distributed transaction'
									END
									,[transaction_state_desc] = CASE database_transaction_state
										WHEN 0 THEN 'The transaction has not been completely initialized yet'
										WHEN 1 THEN 'The transaction has been initialized but has not started'
										WHEN 2 THEN 'The transaction is active'
										WHEN 3 THEN 'The transaction has ended. This is used for read-only transactions'
										WHEN 4 THEN 'The commit process has been initiated on the distributed transaction. This is for distributed transactions only. The distributed transaction is still active but further processing cannot take place'
										WHEN 5 THEN 'The transaction is in a prepared state and waiting resolution.'
										WHEN 6 THEN 'The transaction has been committed'
										WHEN 7 THEN 'The transaction is being rolled back'
										WHEN 8 THEN 'The transaction has been rolled back'
									END
								FROM sys.dm_tran_database_transactions DT
								INNER JOIN sys.dm_tran_session_transactions ST ON DT.transaction_id = ST.transaction_id
								LEFT JOIN sys.databases D ON DT.database_id = D.database_id
								ORDER BY ST.session_id
								option(recompile)";



		/// <summary>
		/// 数据库使用tempdb中的总空间
		/// </summary>
		public static string _sqlstrTempDB_VersionStore_space = @"if(convert(char(12),serverproperty('productversion')) > '11.0')
				begin
				select
				  @@SERVERNAME AS ServerName,
				  DB_NAME(database_id) as 'Database Name',
				  reserved_page_count,
				  reserved_space_kb 
				FROM sys.dm_tran_version_store_space_usage;
				end
				";

		/// <summary>
		/// 备份报告
		/// </summary>
		public static string _sqlstrBackup_info = @"			
							EXEC('	SELECT TOP (2000) 
							@@SERVERNAME as ServerName,
									bs.machine_name,
									bs.server_name,
									bs.database_name AS [Database Name],
									bs.recovery_model,
								CONVERT (FLOAT, bs.backup_size / 1048576.0 ) AS[Uncompressed Backup Size(MB)],
						CONVERT(FLOAT, bs.compressed_backup_size / 1048576.0 ) AS[Compressed Backup Size(MB)],
						CONVERT(NUMERIC (20,2), (CONVERT (FLOAT, bs.backup_size) /
						CONVERT(FLOAT, bs.compressed_backup_size))) AS[Compression Ratio], bs.has_backup_checksums, bs.is_copy_only, bs.encryptor_type,
						DATEDIFF(SECOND, bs.backup_start_date, bs.backup_finish_date) AS[Backup Elapsed Time(sec)],
						bs.backup_start_date AS[Backup Start Date],
						bs.backup_finish_date AS[Backup Finish Date], bmf.physical_device_name AS[Backup Location], bmf.physical_block_size
					  FROM msdb.dbo.backupset AS bs WITH(NOLOCK)
						INNER JOIN msdb.dbo.backupmediafamily AS bmf WITH(NOLOCK) ON bs.media_set_id = bmf.media_set_id
					   where 7 >= DATEDIFF (day, bs.backup_start_date, getdate())
						ORDER BY bs.backup_finish_date DESC OPTION(RECOMPILE);');";



		/// <summary>
		/// /内联函数
		/// </summary>
		public static string _sqlstrinline_function = @"	DECLARE @Version int, @Command NVARCHAR(1000)
							SET @Version=convert (int,REPLACE (LEFT (CONVERT (varchar, SERVERPROPERTY ('ProductVersion')),2), '.', ''))
							IF @Version > '14'
							BEGIN			CREATE TABLE #fnstats
									(
										ServerName		varchar(256),
										DatabaseName	varchar(256),
										FunctionName	varchar(256),
										is_inlineable	bit,
										querytext		bit,
										definition		varchar(2000)
									)
	
									DECLARE @SQL VARCHAR(max)

									SELECT @SQL = '
											use [?];
											if(''[?]'' not in(''master'',''msdb'',''model'',''tempdb''))
											begin
												INSERT INTO #fnstats
												select @@SERVERNAME as ServerName,
												db_name() as DatabaseName,  
												OBJECT_NAME(m.object_id) AS [Function Name], m.is_inlineable, m.inline_type, left(m.definition,2000) as querytext
												FROM sys.sql_modules AS m WITH (NOLOCK) 
												LEFT OUTER JOIN sys.dm_exec_function_stats AS efs WITH (NOLOCK)
												ON  m.object_id = efs.object_id
												WHERE efs.type_desc = N''SQL_SCALAR_FUNCTION'' AND db_name() = ''[?]''
												OPTION (RECOMPILE);
											END
											'
									print @SQL
									EXEC sp_MSforeachdb @SQL;   
									SELECT * FROM  #fnstats
									DROP TABLE #fnstats
								end


						";


		/// <summary>
		/// 数据库锁等待
		/// </summary>
		public static string _sqlstrDB_Lock_Wait = @"CREATE TABLE #DB_Lock_Wait
										(
											ServerName nvarchar(256),
									DatabaseName nvarchar(256),
									table_name nvarchar(256),
									index_name nvarchar(256),
									index_id int,
									partition_number int,
									total_row_lock_waits bigint,
									total_row_lock_wait_in_ms   bigint,
									total_page_lock_waits bigint,
									total_page_lock_wait_in_ms  bigint,
									total_lock_wait_in_ms bigint
								)


								DECLARE @SQL VARCHAR(max)

								SELECT @SQL = '
										use[?];
										if(''[?]'' not in(''master'',''msdb'',''model'',''tempdb''))
										begin
											INSERT INTO #DB_Lock_Wait
											select @@SERVERNAME as ServerName,
											db_name() as DatabaseName,  
											o.name AS[table_name], i.name AS[index_name], ios.index_id, ios.partition_number,
											SUM(ios.row_lock_wait_count) AS[total_row_lock_waits], 
											SUM(ios.row_lock_wait_in_ms) AS[total_row_lock_wait_in_ms],
											SUM(ios.page_lock_wait_count) AS[total_page_lock_waits],
											SUM(ios.page_lock_wait_in_ms) AS[total_page_lock_wait_in_ms],
											SUM(ios.page_lock_wait_in_ms)+ SUM(row_lock_wait_in_ms) AS[total_lock_wait_in_ms]
											FROM sys.dm_db_index_operational_stats(DB_ID(), NULL, NULL, NULL) AS ios
											INNER JOIN sys.objects AS o WITH(NOLOCK) ON ios.[object_id] = o.[object_id]

										   INNER JOIN sys.indexes AS i WITH (NOLOCK) ON ios.[object_id] = i.[object_id]  AND ios.index_id = i.index_id

										   WHERE o.[object_id] > 100 AND db_name() = ''[?]''

										   GROUP BY o.name, i.name, ios.index_id, ios.partition_number
										   HAVING SUM(ios.page_lock_wait_in_ms)+ SUM(row_lock_wait_in_ms) > 0

										   ORDER BY total_lock_wait_in_ms DESC OPTION (RECOMPILE)
									   END
										'
								print @SQL
								EXEC sp_MSforeachdb @SQL;
										SELECT* FROM  #DB_Lock_Wait
								DROP TABLE #DB_Lock_Wait
 												";



		/// <summary>
		/// 可恢复的索引重建
		/// </summary>
		public static string _sqlstrResumable_index_rebuild = @"	DECLARE @Version int, @Command NVARCHAR(1000)
					SET @Version=convert (int,REPLACE (LEFT (CONVERT (varchar, SERVERPROPERTY ('ProductVersion')),2), '.', ''))
					IF @Version > 13
					BEGIN
					-- Get any resumable index rebuild operation information (Query 67) (Resumable Index Rebuild)
							SELECT @@SERVERNAME as ServerName,OBJECT_NAME(iro.object_id) AS [Object Name], iro.index_id, iro.name AS [Index Name],
							iro.sql_text, iro.last_max_dop_used, iro.partition_number, iro.state_desc, iro.start_time, iro.percent_complete
							FROM  sys.index_resumable_operations AS iro WITH (NOLOCK)
							OPTION (RECOMPILE);
					end";


		/// <summary>
		/// 内存Dump
		/// </summary>
		public static string _sqlstrMemory_dump = @"SELECT @@SERVERNAME AS servername,[filename], creation_time, size_in_bytes/1048576.0 AS [Size (MB)]
					FROM sys.dm_server_memory_dumps WITH (NOLOCK)
					ORDER BY creation_time DESC OPTION (RECOMPILE); ";

		
		
		/// <summary>
		/// 磁盘信息
		/// </summary>
		public static string _sqlstrDisk_LUN_Info = @"SELECT DISTINCT @@SERVERNAME AS servername,vs.volume_mount_point, vs.file_system_type, vs.logical_volume_name, 
					CONVERT(DECIMAL(18,2), vs.total_bytes/1073741824.0) AS [Total Size (GB)],
					CONVERT(DECIMAL(18,2), vs.available_bytes/1073741824.0) AS [Available Size (GB)],  
					CONVERT(DECIMAL(18,2), vs.available_bytes * 1. / vs.total_bytes * 100.) AS [Space Free %],
					vs.supports_compression, vs.is_compressed, 
					vs.supports_sparse_files, vs.supports_alternate_streams
					FROM sys.master_files AS f WITH (NOLOCK)
					CROSS APPLY sys.dm_os_volume_stats(f.database_id, f.[file_id]) AS vs 
					ORDER BY vs.volume_mount_point OPTION (RECOMPILE); ";


		/// <summary>
		/// CPUTOP50查询
		/// </summary>
		public static string _sqlstrTop_50_CPU_Exp_Query = @"SELECT TOP 50 
					@@SERVERNAME AS ServerName, DB_NAME(t.[dbid]) AS [Database Name], 
					REPLACE(REPLACE(LEFT(t.[text], 2000), CHAR(10),''), CHAR(13),'') AS [Short Query Text],  
					qs.total_worker_time AS [Total Worker Time], qs.min_worker_time AS [Min Worker Time],
					qs.total_worker_time/qs.execution_count AS [Avg Worker Time], 
					qs.max_worker_time AS [Max Worker Time], 
					qs.min_elapsed_time AS [Min Elapsed Time], 
					qs.total_elapsed_time/qs.execution_count AS [Avg Elapsed Time], 
					qs.max_elapsed_time AS [Max Elapsed Time],
					qs.min_logical_reads AS [Min Logical Reads],
					qs.total_logical_reads/qs.execution_count AS [Avg Logical Reads],
					qs.max_logical_reads AS [Max Logical Reads], 
					qs.execution_count AS [Execution Count],
					CASE WHEN CONVERT(nvarchar(max), qp.query_plan) LIKE N'%<MissingIndexes>%' THEN 1 ELSE 0 END AS [Has Missing Index], 
					qs.creation_time AS [Creation Time]
					--,t.[text] AS [Query Text], qp.query_plan AS [Query Plan] -- uncomment out these columns if not copying results to Excel
					FROM sys.dm_exec_query_stats AS qs WITH (NOLOCK)
					CROSS APPLY sys.dm_exec_sql_text(plan_handle) AS t 
					CROSS APPLY sys.dm_exec_query_plan(plan_handle) AS qp 
					ORDER BY qs.total_worker_time DESC OPTION (RECOMPILE); ";


		/// <summary>
		/// TOP50读查询
		/// </summary>
		public static string _sqlstrTop_50_Reads_Query = @"	SELECT TOP 50 
					@@SERVERNAME AS ServerName, DB_NAME(t.[dbid]) AS [Database Name], 
					REPLACE(REPLACE(LEFT(t.[text], 2000), CHAR(10),''), CHAR(13),'') AS [Short Query Text],  
					qs.total_worker_time AS [Total Worker Time], qs.min_worker_time AS [Min Worker Time],
					qs.total_worker_time/qs.execution_count AS [Avg Worker Time], 
					qs.max_worker_time AS [Max Worker Time], 
					qs.min_elapsed_time AS [Min Elapsed Time], 
					qs.total_elapsed_time/qs.execution_count AS [Avg Elapsed Time], 
					qs.max_elapsed_time AS [Max Elapsed Time],
					qs.min_logical_reads AS [Min Logical Reads],
					qs.total_logical_reads/qs.execution_count AS [Avg Logical Reads],
					qs.max_logical_reads AS [Max Logical Reads], 
					qs.execution_count AS [Execution Count],
					CASE WHEN CONVERT(nvarchar(max), qp.query_plan) LIKE N'%<MissingIndexes>%' THEN 1 ELSE 0 END AS [Has Missing Index], 
					qs.creation_time AS [Creation Time]
					--,t.[text] AS [Query Text], qp.query_plan AS [Query Plan] -- uncomment out these columns if not copying results to Excel
					FROM sys.dm_exec_query_stats AS qs WITH (NOLOCK)
					CROSS APPLY sys.dm_exec_sql_text(plan_handle) AS t 
					CROSS APPLY sys.dm_exec_query_plan(plan_handle) AS qp 
					ORDER BY qs.total_logical_reads DESC OPTION (RECOMPILE);";



		/// <summary>
		/// 频率最高Top50查询
		/// </summary>
		public static string _sqlstrFreq_Exec_Query = @"SELECT TOP(50) @@SERVERNAME AS ServerName, DB_NAME(t.[dbid]) AS [Database Name], LEFT(t.[text], 2000) AS [Short Query Text], qs.execution_count AS [Execution Count],
				qs.total_logical_reads AS [Total Logical Reads],
				qs.total_logical_reads/qs.execution_count AS [Avg Logical Reads],
				qs.total_worker_time AS [Total Worker Time],
				qs.total_worker_time/qs.execution_count AS [Avg Worker Time], 
				qs.total_elapsed_time AS [Total Elapsed Time],
				qs.total_elapsed_time/qs.execution_count AS [Avg Elapsed Time],
				CASE WHEN CONVERT(nvarchar(max), qp.query_plan) LIKE N'%<MissingIndexes>%' THEN 1 ELSE 0 END AS [Has Missing Index], 
				qs.creation_time AS [Creation Time]
				--,t.[text] AS [Complete Query Text], qp.query_plan AS [Query Plan] -- uncomment out these columns if not copying results to Excel
				FROM sys.dm_exec_query_stats AS qs WITH (NOLOCK)
				CROSS APPLY sys.dm_exec_sql_text(plan_handle) AS t 
				CROSS APPLY sys.dm_exec_query_plan(plan_handle) AS qp 
				ORDER BY qs.execution_count DESC OPTION (RECOMPILE); ";



		/// <summary>
		/// 分区信息
		/// </summary>
		public static string _sqlstrPartitioning_Info = @"
						if OBJECT_ID('tempdb..#tmp_databases') is not null
						drop table #tmp_databases;
						if OBJECT_ID('tempdb..#Ind') is not null
						drop table #Ind; 
						CREATE TAble #tmp_databases
						(
							id int identity(1,1),
							dbname varchar(200)
						);
						CREATE TABLE #Ind
						( 
							DatabaseName			varchar(256),
							SchemaName				varchar(256),
							TableName				varchar(256),
							PartitionSchemeName		varchar(256),
							PartitionFilegroupName	varchar(256),
							PartitionFunctionName	varchar(256),
							PartitionFunctionRange	varchar(256),
							PartitionBoundary		varchar(256),
							PartitionBoundaryValue	sql_variant,
							PartitionKey			varchar(256),
							PartitionRange			varchar(256),
							PartitionNumber			int,
							PartitionRowCount		bigint,
							DataCompression			varchar(120)
						);

						insert into #tmp_databases
						select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

						DECLARE @min int=1,@max int =0
						select @max=count(*) from #tmp_databases

						while(@min <=@max)
						begin
							DECLARE @SQL VARCHAR(8000) = ''
							,@dbname varchar(200) = ''

							SELECT @dbname=dbname from #tmp_databases where id = @min


								SELECT @SQL=' USE ['+@dbname+'];
										INSERT INTO #Ind
										SELECT DB_NAME() AS DatabaseName,
										OBJECT_SCHEMA_NAME(pstats.[object_id]) AS SchemaName,
										OBJECT_NAME(pstats.[object_id]) AS TableName,
										ps.[name] AS PartitionSchemeName,
										ds.[name] AS PartitionFilegroupName,
										pf.[name] AS PartitionFunctionName,
										CASE pf.[boundary_value_on_right] WHEN 0 THEN ''Range Left'' ELSE ''Range Right'' END AS PartitionFunctionRange,
										CASE pf.[boundary_value_on_right] WHEN 0 THEN ''Upper Boundary'' ELSE ''Lower Boundary'' END AS PartitionBoundary,
										prv.[value] AS PartitionBoundaryValue,
										c.[name] AS PartitionKey,
										CASE WHEN pf.[boundary_value_on_right] = 0 THEN c.name + '' > '' + CAST (ISNULL(LAG(prv.value) OVER (PARTITION BY pstats.object_id ORDER BY pstats.object_id, pstats.partition_number), ''Infinity'') AS VARCHAR (100)) + '' and '' + c.name + '' <= '' + CAST (ISNULL(prv.value, ''Infinity'') AS VARCHAR (100)) ELSE c.name + '' >= '' + CAST (ISNULL(prv.value, ''Infinity'') AS VARCHAR (100)) + '' and '' + c.name + '' < '' + CAST (ISNULL(LEAD(prv.value) OVER (PARTITION BY pstats.object_id ORDER BY pstats.object_id, pstats.partition_number), ''Infinity'') AS VARCHAR (100)) END AS PartitionRange,
										pstats.[partition_number] AS PartitionNumber,
										pstats.[row_count] AS PartitionRowCount, 		
										p.[data_compression_desc] AS DataCompression
										FROM sys.dm_db_partition_stats AS pstats
										INNER JOIN sys.partitions AS p ON pstats.partition_id = p.partition_id
										INNER JOIN sys.destination_data_spaces AS dds ON pstats.partition_number = dds.destination_id
										INNER JOIN sys.data_spaces AS ds ON dds.data_space_id = ds.data_space_id
										INNER JOIN 	sys.partition_schemes AS ps ON dds.partition_scheme_id = ps.data_space_id
										INNER JOIN sys.partition_functions AS pf ON ps.function_id = pf.function_id
										INNER JOIN sys.indexes AS i ON pstats.object_id = i.object_id AND pstats.index_id = i.index_id
										AND dds.partition_scheme_id = i.data_space_id AND i.type <= 1
										INNER JOIN sys.index_columns AS ic ON i.index_id = ic.index_id AND i.object_id = ic.object_id AND ic.partition_ordinal > 0
										INNER JOIN sys.columns AS c ON pstats.object_id = c.object_id AND ic.column_id = c.column_id
										LEFT OUTER JOIN sys.partition_range_values AS prv ON pf.function_id = prv.function_id AND pstats.partition_number = (CASE pf.boundary_value_on_right WHEN 0 THEN prv.boundary_id ELSE (prv.boundary_id + 1) END)
									'
							--print @SQL
								EXEC (@SQL) 
								set @min = @min+1
							end		
							select @@SERVERNAME AS ServerName,* from #Ind
							DROP TABLE #Ind
							DROP TABLE #tmp_databases

							";



		/// <summary>
		/// 索引大小报告
		/// </summary>
		public static string _sqlstrIndex_Size_Info = @"
						if OBJECT_ID('tempdb..#tmp_databases') is not null
						drop table #tmp_databases;
						if OBJECT_ID('tempdb..#Ind') is not null
						drop table #Ind; 
						CREATE TAble #tmp_databases
						(
							id int identity(1,1),
							dbname varchar(200)
						);
						CREATE TABLE #Ind
						(
							dbname			varchar(100),
							[schema_name]	varchar(256),
							TableName		varchar	(256),
							IndexName		varchar(256),
							IsUnique		varchar(10),
							type_desc		nvarchar(120),
							IndexOptions	varchar	(153),
							is_disabled		bit	,
							FileGroupName	nvarchar(256),
							KeyCol			nvarchar(3000),
							IncludedCol		nvarchar(3000),
							Indexsize_MB	numeric
						);

						insert into #tmp_databases
						select name from sys.databases where is_read_only=0 and state_desc='ONLINE' and name not in ('master','model','tempdb','msdb');

						DECLARE @min int=1,@max int =0
						select @max=count(*) from #tmp_databases

						while(@min <=@max)
						begin
							DECLARE @SQL VARCHAR(8000) = ''
							,@dbname varchar(200) = ''

							SELECT @dbname=dbname from #tmp_databases where id = @min


								SELECT @SQL=' USE ['+@dbname+'];
										INSERT INTO #Ind
										select
										 DB_NAME() as DatabaseName,
										 schema_name(t.schema_id) [schema_name], t.name, ix.name,
										 case when ix.is_unique = 1 then ''UNIQUE '' else '''' END 
										 , ix.type_desc,
										 case when ix.is_padded=1 then ''PAD_INDEX = ON, '' else ''PAD_INDEX = OFF, '' end
										 + case when ix.allow_page_locks=1 then ''ALLOW_PAGE_LOCKS = ON, '' else ''ALLOW_PAGE_LOCKS = OFF, '' end
										 + case when ix.allow_row_locks=1 then  ''ALLOW_ROW_LOCKS = ON, '' else ''ALLOW_ROW_LOCKS = OFF, '' end
										 + case when INDEXPROPERTY(t.object_id, ix.name, ''IsStatistics'') = 1 then ''STATISTICS_NORECOMPUTE = ON, '' else ''STATISTICS_NORECOMPUTE = OFF, '' end
										 + case when ix.ignore_dup_key=1 then ''IGNORE_DUP_KEY = ON, '' else ''IGNORE_DUP_KEY = OFF, '' end
										 + ''SORT_IN_TEMPDB = OFF, FILLFACTOR ='' + CAST(ix.fill_factor AS VARCHAR(3)) AS IndexOptions
										 , ix.is_disabled , fg.name as FileGroupName
										 ,key_columns.*
										 ,includedcolumns.*
										 ,indexspace.Indexsize_MB
							from sys.tables t 
							INNER JOIN sys.schemas sm on t.schema_id = sm.schema_id
							inner join sys.indexes ix on t.object_id=ix.object_id 
							inner join sys.filegroups fg on fg.data_space_id=ix.data_space_id		
							OUTER APPLY
							(
							select KeyCol = (SELECT col.name+'',''
								FROM sys.index_columns ixc 
								inner join sys.columns col on ixc.object_id =col.object_id  and ixc.column_id=col.column_id
								where ix.type>0 and (ix.is_primary_key=0 or ix.is_unique_constraint=0) and  ix.object_id=ixc.object_id and ix.index_id= ixc.index_id	
								and ixc.key_ordinal >0
								order by ixc.key_ordinal
								for xml path(''''))
							)key_columns
							OUTER APPLY
							(
							select IncludedCol = ( SELECT col.name+'',''
									FROM sys.index_columns ixc 
									inner join sys.columns col on ixc.object_id =col.object_id  and ixc.column_id=col.column_id
									where ix.type>0 and (ix.is_primary_key=0 or ix.is_unique_constraint=0) and  ix.object_id=ixc.object_id and ix.index_id= ixc.index_id	
									and ixc.is_included_column >0
									order by ixc.key_ordinal
									for xml path('''')
									)
							)includedcolumns 
							LEFT JOIN
							(
								SELECT tn.OBJECT_ID as tblobj, ix.name AS indexobj,
								(SUM(sz.[used_page_count]) * 8.0)/1024.0 AS [Indexsize_MB]
								FROM sys.dm_db_partition_stats AS sz
								INNER JOIN sys.indexes AS ix ON sz.[object_id] = ix.[object_id] 	AND sz.[index_id] = ix.[index_id]
								INNER JOIN sys.tables tn ON tn.OBJECT_ID = ix.object_id
								GROUP BY tn.OBJECT_ID , ix.name
							)indexspace ON t.object_id=indexspace.tblobj   AND indexspace.indexobj=ix.name
							inner join information_schema.tables it on ((it.[TABLE_SCHEMA]+''.''+it.[TABLE_NAME]) = (sm.[name]+''.''+t.[name]))
							where  ix.type>0 --and ix.is_primary_key=0 and ix.is_unique_constraint=0 --and schema_name(tb.schema_id)= @SchemaName and tb.name=@TableName
							--and t.is_ms_shipped=0 and t.name<>''sysdiagrams''
									'
							--print @SQL
								EXEC (@SQL) 
								set @min = @min+1
							end		
							select @@SERVERNAME AS ServerName,* from #Ind
							DROP TABLE #Ind
							DROP TABLE #tmp_databases
						";


		/// <summary>
		/// 最后备份信息
		/// </summary>
		public static string _sqlstrLast_Backupup_Info = @"
							SET quoted_identifier OFF
							DECLARE @dbname AS VARCHAR(80)
							DECLARE @msgdb AS VARCHAR(100)
							DECLARE @dbbkpname AS VARCHAR(80)
							DECLARE @dypart1 AS VARCHAR(2)
							DECLARE @dypart2 AS VARCHAR(3)
							DECLARE @dypart3 AS VARCHAR(4)
							DECLARE @currentdate AS VARCHAR(10)
							DECLARE @server_name AS VARCHAR(30)
							SELECT @server_name = @@servername
							SELECT @dypart1 = DATEPART(dd,GETDATE())
							SELECT @dypart2 = DATENAME(mm,GETDATE())
							SELECT @dypart3 = DATEPART(yy,GETDATE())
							SELECT @currentdate= @dypart1 + @dypart2 + @dypart3

							SELECT @@SERVERNAME ServerName,SUBSTRING(s.name,1,50) AS 'DATABASE Name',
							b.backup_start_date AS 'Full DB Backup Status',
							c.backup_start_date AS 'Differential DB Backup Status',
							d.backup_start_date AS 'Transaction Log Backup Status'
							FROM MASTER..sysdatabases s
							LEFT OUTER JOIN msdb..backupset b
							ON s.name = b.database_name
							AND b.backup_start_date =
							(SELECT MAX(backup_start_date)AS 'Full DB Backup Status'
							FROM msdb..backupset
							WHERE database_name = b.database_name
							AND TYPE = 'D') -- full database backups only, not log backups
							LEFT OUTER JOIN msdb..backupset c
							ON s.name = c.database_name
							AND c.backup_start_date =
							(SELECT MAX(backup_start_date)'Differential DB Backup Status'
							FROM msdb..backupset
							WHERE database_name = c.database_name
							AND TYPE = 'I')
							LEFT OUTER JOIN msdb..backupset d
							ON s.name = d.database_name
							AND d.backup_start_date =
							(SELECT MAX(backup_start_date)'Transaction Log Backup Status'
							FROM msdb..backupset
							WHERE database_name = d.database_name
							AND TYPE = 'L')
							WHERE s.name <>'tempdb'
							ORDER BY s.name";

		//public static string _sqlstrAG_Listener_IP213 = @"";




		//public static string _sqlstrAG_Listener_IP213 = @"";

		//public static string _sqlstrAG_Listener_IP213 = @"";

	}
}
