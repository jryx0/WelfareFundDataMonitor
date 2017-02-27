--
-- 由SQLiteStudio v3.1.0 产生的文件 周一 2月 27 11:15:08 2017
--
-- 文本编码：System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- 表：RulesTmp
DROP TABLE IF EXISTS RulesTmp;

CREATE TABLE RulesTmp (
    RowID    INTEGER       PRIMARY KEY AUTOINCREMENT,
    TmpName  VARCHAR (20),
    Rules    VARCHAR (500),
    Rule2    VARCHAR (500),
    Rule3    VARCHAR (500),
    PreRules VARCHAR (500),
    Type     INTEGER (2),
    Status   INTEGER       DEFAULT (1),
    Seq      INTEGER       DEFAULT (9999999) 
);

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         1,
                         '源比双表一对多模板',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.Region AS 乡镇街道,
       @table1.ID AS 身份证号,
       @table2.Name AS @t2s姓名,
       @table2.Addr AS @t2s地址,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,
       @table1.sDataDate AS 领取时间,
       @table1.Amount AS 领取金额
  FROM @table1
       JOIN
      @table2 ON @table1.ID = @table2.ID
 WHERE  ((@table1.ID) = 15 OR 
          length(@table1.ID) = 18)  and  @table1.Amount>= @para
 ORDER BY 乡镇街道,@t1s地址,
          身份证号,领取时间 
 
',
                         'SELECT distinct 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,       
       group_concat(distinct @t2s姓名) @t2s姓名,  
        @t1s地址,
       min(date(领取时间) ) 开始领取时间,     
       max(date(领取时间) ) 最后领取时间,
       sum(领取金额) 金额,       
       group_concat(distinct @t2s地址) 备注       
  FROM @tablename
 GROUP BY 类型,身份证号           
 ORDER BY 乡镇街道,@t1s地址,身份证号
',
                         'SELECT distinct 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,
       @t1s地址,  
       min(领取时间) ||'' 至 '' || max(领取时间) 领取时间,
      ''@t2s地址:''  || ifnull(group_concat(distinct @t2s地址), '''') 备注 @injection  
  FROM @tablename
 GROUP BY 类型,         
          乡镇街道,
          身份证号           
 ORDER BY 乡镇街道,@t1s地址,身份证号',
                         '',
                         2,
                         1,
                         100
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         2,
                         '源比双表多对多模板',
                         'SELECT DISTINCT ''@aimtype'' AS 类型,
                @table1.Region AS 乡镇街道,
                @table1.ID AS 身份证号,
                @table1.Name AS @t1s姓名,
                b.Name AS @t2s姓名,
                @table1.Addr AS @t1s地址,
                @table1.sDataDate AS 领取时间,
                @table1.Amount AS 领取金额,
                b.型号 AS 备注
  FROM @table1,
       (SELECT DISTINCT ID,
                        Name,
                        group_concat(Type, ''/'') AS 型号
          FROM @table2
         GROUP BY ID,
                  Name) b
 WHERE ( (length(@table1.ID) = 15 OR 
          length(@table1.ID) = 18) ) AND 
       b.ID = @table1.ID
 ORDER BY 乡镇街道,@t1s地址,
          身份证号 ,领取时间',
                         'SELECT distinct 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,  
       group_concat(distinct @t2s姓名) @t2s姓名,
       @t1s地址,
       min(date(领取时间) ) 开始领取时间,     
       max(date(领取时间) ) 最后领取时间,
       sum(领取金额) 金额,
       备注     
  FROM @tablename
 GROUP BY 类型,身份证号,备注
 ORDER BY 乡镇街道,@t1s地址,身份证号',
                         'SELECT distinct 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,  
       @t1s地址,
       min(领取时间) ||'' 至 '' || max(领取时间) 领取时间,
        ''@t2s姓名:'' || @t2s姓名 || '','' || ifnull(substr(备注, 1, 100), '''') 备注 @injection 
  FROM @tablename
 GROUP BY 类型,身份证号,备注
 ORDER BY 乡镇街道,@t1s地址,身份证号

',
                         NULL,
                         2,
                         1,
                         200
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         3,
                         '源比比三表户籍模板',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.Region AS 乡镇街道,
       @table1.ID AS 身份证号,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,
       @table1.sDataDate AS 领取时间,
       @table1.Amount AS 领取金额,
       b.ID AS @t2s身份证号,
       b.Name AS @t2s姓名,
       b.备注 AS 备注,
       @table3.Relation AS 亲属关系
  FROM @table1,
       @table3,
       (SELECT ID,
               Name,
               group_concat(Type, ''/'') AS 备注
          FROM @table2 where (length(ID) = 15 OR 
        length(ID) = 18)
         GROUP BY ID,
                  Name) b
 WHERE @table3.sRelateID = @table1.ID AND 
       (@table3.Relation IN (''夫'',
       ''妻'',
       ''配偶'',
       ''子'',
       ''女'',
       ''长子'',
       ''长女'',
       ''二子'',
       ''二女'',
       ''次子'',
       ''次女'',
       ''父'',
       ''母'',
       ''父母'') ) AND 
       @table3.ID =  b.ID AND 
       (length(@table1.ID) = 15 OR 
        length(@table1.ID) = 18) 
 ORDER BY 乡镇街道,@t1s地址,
         身份证号,领取时间',
                         'SELECT DISTINCT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名,      
      group_concat(distinct @t2s姓名) @t2s姓名,
      @t1s地址,
      group_concat(distinct @t2s身份证号) @t2s身份证号,
      min(date(领取时间) ) 开始领取时间,     
      max(date(领取时间) ) 最后领取时间,
      sum(领取金额) 金额,
      group_concat(distinct 亲属关系) 亲属关系 ,
      group_concat(distinct 备注) 备注
  FROM @tablename
 GROUP BY 类型,          
          身份证号
 ORDER BY 乡镇街道,@t1s地址,
          身份证号
',
                         'SELECT DISTINCT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名,  
      @t1s地址,    
      min(领取时间) ||'' 至 '' || max(领取时间) 领取时间,  
     ''@t2s姓名:''|| ifnull(group_concat(distinct @t2s姓名), '''') ||
      ifnull( group_concat(distinct @t2s身份证号), '''')  || 
     '',关系:''||   ifnull(group_concat(distinct 亲属关系), '''') || 
     '', '' ||  ifnull(group_concat(distinct 备注), '''')  备注 @injection 
  FROM @tablename
 GROUP BY 类型,          
          身份证号
 ORDER BY 乡镇街道,@t1s地址,
          身份证号',
                         '',
                         3,
                         1,
                         300
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         4,
                         '身份姓名不一致模板',
                         'SELECT DISTINCT ''@aimtype'' AS 类型,
                @table1.Region AS 乡镇街道,
                @table1.ID AS 身份证号,
                @table1.Name AS @t1s姓名,
                @table1.Addr AS @t1s地址,
                tbComparePersonInfo.Name AS 公安姓名,
                @table1.sDataDate AS 领取时间,
                @table1.Amount AS 领取金额,
                @table1.Type AS 备注
  FROM @table1
       JOIN
       tbComparePersonInfo ON @table1.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> @table1.Name AND 
       (length(@table1.ID) = 15 OR 
        length(@table1.ID) = 18) 
 ORDER BY 乡镇街道,          
          @t1s地址,
          身份证号,领取时间',
                         'SELECT DISTINCT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名 ,
      group_concat(distinct 公安姓名) 公安姓名 ,
      @t1s地址, 
      min(date(领取时间) ) 开始领取时间,     
      max(date(领取时间) ) 最后领取时间,
      sum(领取金额) 金额 ,
      ifnull(group_concat(备注), '''') as 备注   
  FROM @tablename
 GROUP BY 类型,           
          身份证号           
 ORDER BY 乡镇街道,
          @t1s地址,
          身份证号',
                         'SELECT DISTINCT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名 ,      
      @t1s地址, 
      min(领取时间) ||'' 至 '' || max(领取时间) 领取时间,
      ''公安姓名:'' || ifnull(group_concat(distinct 公安姓名), '''') || '','' || ifnull(备注, '''')  as 备注  
     @injection 
  FROM @tablename
 GROUP BY 类型,           
          身份证号           
 ORDER BY 乡镇街道,
          @t1s地址,
          身份证号

',
                         '',
                         1,
                         1,
                         400
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         5,
                         '死亡模板',
                         'SELECT 类型,
       乡镇街道,
       身份证号,
       @t1s姓名,
       @t1s地址,
       领取时间,
       领取金额,
       死者身份证号,
       死者姓名,
       死亡时间
  FROM (SELECT ''@aimtype'' AS 类型,
               @table1.Region AS 乡镇街道,
               @table1.ID AS 身份证号,
               @table1.Name AS @t1s姓名,
               @table1.Addr AS @t1s地址,
               @table1.sDataDate AS 领取时间,
               @table1.Amount AS 领取金额,
               tbCompareBurnInfo.InputID AS 死者身份证号,
               tbCompareBurnInfo.Name AS 死者姓名,
               tbCompareBurnInfo.sDataDate AS 死亡时间
          FROM @table1
               JOIN
               tbCompareBurnInfo ON @table1.ID = tbCompareBurnInfo.ID
         WHERE tbCompareBurnInfo.sDataDate < @table1.sDataDate AND 
               (length(@table1.ID) = 15 OR 
                length(@table1.ID) = 18) 
       UNION
       SELECT ''@aimtype'' AS 类型,
              @table1.Region AS 乡镇街道,
              @table1.ID AS 身份证号,
              @table1.Name AS @t1s姓名,
              @table1.Addr AS @t1s地址,
              @table1.sDataDate AS 领取时间,
              @table1.Amount AS 领取金额,
              tbCompareDeathInfo.InputID AS 死者身份证号,
              tbCompareDeathInfo.Name AS 死者姓名,
              tbCompareDeathInfo.sDataDate AS 死亡时间
         FROM @table1
              JOIN
              tbCompareDeathInfo ON @table1.ID = tbCompareDeathInfo.ID
        WHERE tbCompareDeathInfo.sDataDate < @table1.sDataDate AND 
              (length(@table1.ID) = 15 OR 
               length(@table1.ID) = 18) 
        ORDER BY 乡镇街道,
                 身份证号) 
 WHERE 身份证号 IN (SELECT DISTINCT 身份证号
                  FROM (SELECT @table1.ID AS 身份证号,
                               @table1.sDataDate AS 领取时间,
                               tbCompareBurnInfo.sDataDate AS 死亡时间
                          FROM @table1
                               JOIN
                               tbCompareBurnInfo ON @table1.ID = tbCompareBurnInfo.ID
                         WHERE tbCompareBurnInfo.sDataDate < @table1.sDataDate AND 
                               (length(@table1.ID) = 15 OR 
                                length(@table1.ID) = 18) 
                       UNION
                       SELECT @table1.ID AS 身份证号,
                              @table1.DataDate AS 领取时间,
                              tbCompareDeathInfo.sDataDate AS 死亡时间
                         FROM @table1
                              JOIN
                              tbCompareDeathInfo ON @table1.ID = tbCompareDeathInfo.ID
                        WHERE tbCompareDeathInfo.sDataDate < @table1.sDataDate AND 
                              (length(@table1.ID) = 15 OR 
                               length(@table1.ID) = 18) ) 
                 WHERE julianday(领取时间) - julianday(死亡时间) > @para AND 
               死亡时间 > ''2010-01-01'')  
order by 乡镇街道,
       身份证号 ,领取时间',
                         'SELECT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名 ,
      @t1s地址, 
      min(date(领取时间) ) 开始领取时间,     
      max(date(领取时间) ) 最后领取时间,
      sum(领取金额) 金额 ,
      group_concat(distinct 死者身份证号) 死者身份证号,
      group_concat(distinct  死者姓名) 死者姓名,
      max(死亡时间),
      CAST (julianday(领取时间) - julianday(死亡时间) AS INT) AS 超领天数
  FROM @tablename
 WHERE 超领天数 > @para
 GROUP BY 类型,
          乡镇街道,
          身份证号
 ORDER BY 乡镇街道,
          超领天数 DESC,
          身份证号',
                         'SELECT DISTINCT 类型,
                乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址,
                min(领取时间) || '' 至 '' || max(领取时间) 领取时间,
                sum(领取金额) 总金额 @injection
  FROM @tablename
 GROUP BY 类型,@t1s姓名,
          身份证号
 ORDER BY 乡镇街道,
         @t1s地址,
          身份证号',
                         '',
                         2,
                         1,
                         500
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         7,
                         '身份证未查到模板',
                         'SELECT DISTINCT ''@aimtype'' AS 类型,
                @table1.Region AS 乡镇街道,
                @table1.ID AS 身份证号,
                @table1.Addr AS @t1s地址,
                @table1.Name AS @t1s姓名,
                @table1.sDataDate AS 领取时间,
                @table1.Amount AS 领取金额,
                @table1.Type AS 备注
  FROM @table1
 WHERE @table1.ID NOT IN (SELECT tbComparePersonInfo.ID
                                              FROM tbComparePersonInfo) AND 
       (length(@table1.ID) = 15 OR 
        length(@table1.ID) = 18) 
 ORDER BY 乡镇街道,
          @t1s地址,
          身份证号,领取时间',
                         'SELECT DISTINCT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名 ,
      @t1s地址,       
      min(date(领取时间) ) 开始领取时间,     
      max(date(领取时间) ) 最后领取时间,
      sum(领取金额) 金额 ,
      ifnull(group_concat(备注), '''')  as 备注      
  FROM @tablename
 GROUP BY 类型,          
          身份证号            
 ORDER BY 乡镇街道,@t1s地址, 身份证号',
                         'SELECT DISTINCT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名 ,
      @t1s地址,       
      min(领取时间) || '' 至 '' || max(领取时间) 领取时间,
      ifnull(group_concat(备注), '''') as 备注              @injection 
  FROM @tablename
 GROUP BY 类型,          
          身份证号            
 ORDER BY 乡镇街道,@t1s地址, 身份证号
',
                         '',
                         1,
                         1,
                         450
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         8,
                         '源比比三表模板',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.region AS 乡镇街道,      
       @table1.ID AS 身份证号,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,
       @table1.sdataDate AS 领取时间,
       @table1.Amount AS 领取金额,
       a.sRelateID AS @t3s身份证号,
       a.RelateName AS @t3s姓名,
       a.Relation AS 关系
  FROM @table1,
       (SELECT DISTINCT @table2.ID,
                        @table2.Name,
                        @table2.sRelateID,
                        @table2.RelateName,
                        @table2.Relation,
                        @table3.Region
          FROM @table2
             Left  JOIN @table3 ON @table2.RelateID = @table3.ID) a
 WHERE @table1.ID <> '''' AND 
       @table1.ID = a.ID AND 
       @table1.Amount > @para
 ORDER BY 乡镇街道,
          @t1s地址,
          身份证号,
          领取时间',
                         'SELECT 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,      
       @t1s地址,
       min(date(领取时间) ) 开始领取时间,     
      max(date(领取时间) ) 最后领取时间,
      sum(领取金额) 金额,    
       ''@t3s身份证号:'' ||ifnull(@t3s身份证号, '''') || '',@t3s姓名:'' || ifnull(@t3s姓名, '''') || '','' || ifnull(关系, '''')    as 备注
  FROM @tablename
 GROUP BY 类型,
       身份证号,
       @t3s姓名,      
      @t3s身份证号,        
       关系 
 ORDER BY 乡镇街道,身份证号',
                         'SELECT 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,      
       @t1s地址,
       min(领取时间) ||'' 至 '' || max(领取时间) 领取时间,        
       ''@t3s身份证号:'' ||ifnull(@t3s身份证号, '''') || '',@t3s姓名:'' || ifnull(@t3s姓名, '''') || '',关系:'' || ifnull(关系, '''')    as 备注 @injection 
  FROM @tablename
 GROUP BY 类型,
       身份证号,
       @t3s姓名,      
      @t3s身份证号,        
       关系 
 ORDER BY 乡镇街道,身份证号
',
                         NULL,
                         4,
                         1,
                         550
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         9,
                         '源源双表时间判断模板',
                         'SELECT ''@aimtype'' 类型,
       a.乡镇街道  乡镇街道,
       a.身份证号 身份证号,
       a.@t1s姓名 @t1s姓名,
       a.@t1s地址 @t1s地址,
       a.@t1s领取年份 @t1s领取年份,
       a.@t1s领取金额 @t1s金额,        
       strftime(''%Y'', a.@t1s领取年份) || ''年:'' || b.@t2s金额 || ''元''  备注
  FROM
       (SELECT DISTINCT group_concat(DISTINCT @table1.Region) AS 乡镇街道,
                        @table1.ID AS 身份证号,
                        group_concat(DISTINCT @table1.Name) AS @t1s姓名,
                        group_concat(DISTINCT @table1.Addr) AS @t1s地址,
                        date(@table1.sDataDate, ''start of year'') AS @t1s领取年份,
                        sum(@table1.Amount) as @t1s领取金额
          FROM @table1
         GROUP BY 身份证号,
                  @t1s领取年份) a left join
       (SELECT DISTINCT @table2.ID AS 身份证号,
                        date(@table2.sDataDate, ''start of year'') AS @t2s领取年份,
                        sum(@table2.Amount) AS @t2s金额
          FROM @table2
         WHERE (length(@table2.ID) = 15 OR 
                length(@table2.ID) = 18)  
               
         GROUP BY 身份证号,
                  @t2s领取年份) b on  a.身份证号 = b.身份证号
 WHERE  
       a.@t1s领取年份 = b.@t2s领取年份 AND 
       a.@t1s领取年份 > date(''2013-01-01'') AND 
       b.@t2s领取年份 > date(''2013-01-01'') and b.@t2s金额 > @para
 ORDER BY a.乡镇街道, a.身份证号',
                         'SELECT DISTINCT 类型,
                乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址 ,                
                group_concat( 备注, ''，'') as 备注
  FROM @tablename
group by 类型,
                乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址',
                         'SELECT DISTINCT 类型,                
                group_concat(distinct 乡镇街道) 乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) @t1s姓名,      
                @t1s地址,
                @t1s领取年份 领取时间,
                group_concat( 备注, ''，'') as 备注 @injection 
  FROM @tablename
group by 类型,
                乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址',
                         NULL,
                         5,
                         1,
                         600
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         10,
                         '源源双表时间判断模板-2',
                         'SELECT DISTINCT ''@aimtype'' AS 类型,
                a.乡镇街道 as 乡镇街道,
                a.身份证号 as 身份证号,
                a.姓名 AS @t1s姓名,
                a.地址 as @t1s地址,
                a.最早领取时间 AS @t1s最早领取时间,
                a.最晚领取时间 AS @t1s最晚领取时间,
                a.金额 AS @t1s领取金额,
                b.最早领取时间 AS @t2s最早领取时间,
                b.最晚领取时间 AS @t2s最晚领取时间,
                b.金额 AS @t2s领取金额
  FROM (
           SELECT group_concat(DISTINCT @table1.Region) AS 乡镇街道,
                  @table1.ID AS 身份证号,
                  group_concat(DISTINCT @table1.Name) AS 姓名,
                  @table1.addr AS 地址,
                  min(date(@table1.sDataDate) ) AS 最早领取时间,
                  max(date(@table1.sDataDate) ) AS 最晚领取时间,
                  sum(@table1.Amount) AS 金额
             FROM @table1
            WHERE length(id) > 1
            GROUP BY 身份证号
       )
       a,
       (
           SELECT group_concat(DISTINCT @table2.Region) AS 乡镇街道,
                  @table2.ID AS 身份证号,
                  group_concat(DISTINCT @table2.Name) AS 姓名,
                  min(date(@table2.sDataDate) ) AS 最早领取时间,
                  max(date(@table2.sDataDate) ) AS 最晚领取时间,
                  sum(@table2.Amount) AS 金额
             FROM @table2
            WHERE length(id) > 1
            GROUP BY 身份证号
       )
       b
 WHERE a.身份证号 = b.身份证号 AND 
       (@t1s最晚领取时间 BETWEEN @t2s最早领取时间 AND @t2s最晚领取时间 OR 
        @t1s最早领取时间 BETWEEN @t2s最早领取时间 AND @t2s最晚领取时间) AND 
       @t2s领取金额 > 0
 ORDER BY 乡镇街道,
          身份证号;
',
                         'SELECT DISTINCT 类型,
                group_concat(distinct 乡镇街道) 乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名)  @t1s姓名,
                @t1s地址,
                ''@t1s领取时间:''||@t1s最早领取时间 ||'' 至 ''|| @t1s最晚领取时间 || '', @t2s领取时间: ''  ||  @t2s最早领取时间 || '' 至 ''|| @t2s最晚领取时间 as 领取时间,
                sum(@t1s领取金额) + sum(@t2s领取金额)  备注  
  FROM @tablename
 GROUP BY 类型,@t1s姓名,
          身份证号
 ORDER BY 乡镇街道,
          @t1s地址,
          身份证号',
                         'SELECT DISTINCT 类型,
                group_concat(distinct 乡镇街道) 乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名)  @t1s姓名,
                @t1s地址,
                ''@t1s领取时间:''||@t1s最早领取时间 ||'' 至 ''|| @t1s最晚领取时间 || '', @t2s领取时间: ''  ||  @t2s最早领取时间 || '' 至 ''|| @t2s最晚领取时间 as 领取时间,
                ''总金额:'' || (sum(@t1s领取金额) + sum(@t2s领取金额))  备注 @injection
  FROM @tablename
 GROUP BY 类型,@t1s姓名,
          身份证号
 ORDER BY 乡镇街道,
          @t1s地址,
          身份证号',
                         '',
                         6,
                         1,
                         700
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         11,
                         '公共卫生双字段重复计数',
                         'SELECT ''@aimtype'' AS 类型,
       tbSourceMedicalAd.Region 乡镇街道,
       tbSourceMedicalAd.ID 身份证号,
       tbSourceMedicalAd.Name 姓名,
       tbSourceMedicalAd.Addr 地址,
       tbSourceMedicalAd.Type  疾病,
       tbSourceMedicalAd.sRelateID 医生身份证 ,
       tbSourceMedicalAd.RelateName  医生 ,
       tbSourceMedicalAd.DataDate 服务时间  
  FROM tbSourceMedicalAd
 WHERE ID IN (
           SELECT ID
             FROM tbSourceMedicalAd
            GROUP BY ID 
           HAVING count(id) >= @para
       )
 ORDER BY srelateid,
          ID',
                         'SELECT DISTINCT 类型,
                group_concat(DISTINCT 乡镇街道) 乡镇街道,
                身份证号,
                group_concat(DISTINCT 姓名) @t1s姓名,
                地址,
                min(date(服务时间) ) 起始服务时间,     
               max(date(服务时间) ) 最后服务时间,
                ''医生:'' || group_concat(DISTINCT 医生) || '', 疾病:'' ||group_concat(DISTINCT 疾病)     备注
  FROM @tablename        
 GROUP BY 类型,
          身份证号
 ORDER BY 乡镇街道,
          身份证号',
                         'SELECT DISTINCT 类型,
                group_concat(DISTINCT 乡镇街道) 乡镇街道,
                身份证号,
                group_concat(DISTINCT 姓名) @t1s姓名,
                地址,
                min(date(服务时间) ) ||''-'' || max(date(服务时间) )  领取时间,
                  ''医生:'' || group_concat(DISTINCT 医生) || '', 疾病:'' ||group_concat(DISTINCT 疾病)     备注 @injection 
  FROM @tablename        
 GROUP BY 类型,
          身份证号
 ORDER BY 乡镇街道,
          身份证号',
                         '',
                         2,
                         1,
                         800
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         12,
                         '源源双表时间判断模板-1',
                         'SELECT DISTINCT ''@aimtype'' AS 类型,
                a.乡镇街道,
                a.身份证号,
                a.姓名 AS @t1s姓名,
                a.@t1s地址,
                a.@t1s最早领取时间 AS 集中供养最早领取时间,
                a.@t1s最晚领取时间 AS 集中供养最晚领取时间,
                a.@t1s金额 AS 集中领取金额,
                b.@t2s最早领取时间 AS 分散供养最早领取时间,
                b.@t2s最晚领取时间 AS 分散供养最晚领取时间,
                b.@t2s金额 AS 分散领取金额
  FROM
       (SELECT DISTINCT @table1.Region AS 乡镇街道,
                        @table1.ID AS 身份证号,
                        @table1.Name AS 姓名,
                        @table1.addr AS @t1s地址,
                        min(date(@table1.sDataDate) ) AS @t1s最早领取时间,
                        max(date(@table1.sDataDate) ) AS @t1s最晚领取时间,
                        sum(@table1.Amount) AS @t1s金额
          FROM @table1 where @table1.AmountType like ''%分散%''
         GROUP BY 乡镇街道,
                  姓名,
                  身份证号) a,
       (SELECT DISTINCT @table2.Region AS 乡镇街道,
                        @table2.ID AS 身份证号,
                        @table2.Name AS 姓名,
                        min(date(@table2.sDataDate) ) AS @t2s最早领取时间,
                        max(date(@table2.sDataDate) ) AS @t2s最晚领取时间,
                        sum(@table2.Amount) AS @t2s金额
          FROM @table2 where @table2.AmountType like ''%集中%''
         GROUP BY 乡镇街道,
                  姓名,
                  身份证号) b
 WHERE a.身份证号 = b.身份证号 AND 
       (length(a.身份证号) = 15 OR 
        length(a.身份证号) = 18) AND 
       (a.@t1s最晚领取时间 BETWEEN b.@t2s最早领取时间 AND b.@t2s最晚领取时间 OR 
        a.@t1s最早领取时间 BETWEEN b.@t2s最早领取时间 AND b.@t2s最晚领取时间)
 ORDER BY a.乡镇街道,
          a.身份证号
',
                         'SELECT DISTINCT 类型,
                group_concat(distinct 乡镇街道)as 乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) as @t1s姓名,
                @t1s地址,   
                ''集中供养:'' || 集中供养最早领取时间 || ''至'' || 集中供养最晚领取时间 || 
                '', 分散供养:'' || 分散供养最早领取时间 || ''至'' || 分散供养最晚领取时间 as 备注
  FROM @tablename
group by 身份证号,集中供养最早领取时间, 集中供养最晚领取时间,分散供养最早领取时间,分散供养最晚领取时间',
                         'SELECT DISTINCT 类型,
                group_concat(distinct 乡镇街道)as 乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) as @t1s姓名,
                @t1s地址,                   
                ''集中供养:'' || 集中供养最早领取时间 || ''至'' || 集中供养最晚领取时间 || 
                '', 分散供养:'' || 分散供养最早领取时间 || ''至'' || 分散供养最晚领取时间 领取时间,
                '''' 备注 @injection 
  FROM @tablename
group by 身份证号,集中供养最早领取时间, 集中供养最晚领取时间,分散供养最早领取时间,分散供养最晚领取时间',
                         NULL,
                         5,
                         1,
                         900
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         13,
                         '重点人群比对模板(无金额)',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.Region AS 乡镇街道,
       @table1.ID AS 身份证号,
       @table2.Name AS @t2s姓名,
       @table2.Addr AS @t2s单位,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,
       @table1.sDataDate AS 领取时间,
       @table1.Amount AS 领取金额
  FROM @table1
       JOIN
      @table2 ON @table1.ID = @table2.ID
 WHERE  ((@table1.ID) = 15 OR 
          length(@table1.ID) = 18) 
 ORDER BY 乡镇街道,@t1s地址,
          身份证号,领取时间
',
                         'SELECT distinct 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,       
       group_concat(distinct @t2s姓名) @t2s姓名,  
        @t1s地址,
       min(date(领取时间) ) 开始领取时间,     
       max(date(领取时间) ) 最后领取时间,
       sum(领取金额) 金额,       
       group_concat(distinct @t2s单位) 备注       
  FROM @tablename
 GROUP BY 类型,身份证号           
 ORDER BY 乡镇街道,@t1s地址,身份证号',
                         'SELECT distinct 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,
       @t1s地址,  
       min(领取时间) ||'' 至 '' || max(领取时间) 领取时间,
      ''@t2s单位:''  || ifnull(group_concat(distinct @t2s单位), '''') 备注 @injection  
  FROM @tablename
 GROUP BY 类型,         
          乡镇街道,
          身份证号           
 ORDER BY 乡镇街道,@t1s地址,身份证号',
                         NULL,
                         2,
                         1,
                         1000
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         14,
                         '重点人群家属比对模板(无金额)',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.region AS 乡镇街道,      
       @table1.ID AS 身份证号,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,
       
       a.sRelateID AS @t3s身份证号,
       a.RelateName AS @t3s姓名,
       a.Relation AS 关系
  FROM @table1,
       (SELECT DISTINCT @table2.ID,
                        @table2.Name,
                        @table2.sRelateID,
                        @table2.RelateName,
                        @table2.Relation,
                        @table3.Region
          FROM @table2
             Left  JOIN @table3 ON @table2.RelateID = @table3.ID) a
 WHERE @table1.ID <> '''' AND 
       @table1.ID = a.ID 
 ORDER BY 乡镇街道,
          @t1s地址,
          身份证号',
                         'SELECT 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,      
       group_concat(distinct @t1s地址) @t1s地址,    
       ''@t3s身份证号:'' ||ifnull(@t3s身份证号, '''') || '',@t3s姓名:'' || ifnull(@t3s姓名, '''') || '','' || ifnull(关系, '''')    as 备注
  FROM @tablename
 GROUP BY 类型,
       身份证号
 ORDER BY 乡镇街道,身份证号',
                         'SELECT 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,      
       @t1s地址,
       '''' 领取时间,        
       ''@t3s身份证号:'' ||ifnull(@t3s身份证号, '''') || '',@t3s姓名:'' || ifnull(@t3s姓名, '''') || '',关系:'' || ifnull(关系, '''')    as 备注 @injection 
  FROM @tablename
 GROUP BY 类型,
       身份证号,
       @t3s姓名,      
      @t3s身份证号,        
       关系 
 ORDER BY 乡镇街道,身份证号
',
                         '',
                         2,
                         1,
                         1100
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         15,
                         '身份死亡验证(无时间)',
                         'select  ''@aimtype'' AS 类型,
               @table1.Region AS 乡镇街道,
               @table1.ID AS 身份证号,
               @table1.Name AS @t1s姓名,
               @table1.Addr AS @t1s地址,
               
               tbCompareDeathInfo.InputID AS 死者身份证号,
               tbCompareDeathInfo.Name AS 死者姓名 
               from @table1 join tbCompareDeathInfo   
on tbCompareDeathInfo.ID = @table1.ID
where  length( @table1.ID )>1
union
select  ''@aimtype'' AS 类型,
               @table1.Region AS 乡镇街道,
               @table1.ID AS 身份证号,
               @table1.Name AS @t1s姓名,
               @table1.Addr AS @t1s地址,
               
               tbCompareBurnInfo.InputID AS 死者身份证号,
               tbCompareBurnInfo.Name AS 死者姓名 
               from @table1 join tbCompareBurnInfo   
on tbCompareBurnInfo.ID = @table1.ID
where  length( @table1.ID )>1',
                         'SELECT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名 ,
      @t1s地址, 
       
      group_concat(distinct 死者身份证号) 死者身份证号,
      group_concat(distinct  死者姓名) 死者姓名 
     
  FROM @tablename
  
 GROUP BY 类型,
           
          身份证号
 ORDER BY 乡镇街道,
        
          身份证号',
                         'SELECT DISTINCT 类型,
                 group_concat(distinct 乡镇街道) 乡镇街道,
                 身份证号,
                 group_concat(distinct @t1s姓名) @t1s姓名 ,
                 @t1s地址, 
                '''' 领取时间,
                ''死者身份证号:'' || group_concat(distinct 死者身份证号) ||  ''死者姓名:'' ||
      group_concat(distinct  死者姓名)  备注 @injection
  FROM @tablename
 GROUP BY 类型,@t1s姓名,
          身份证号
 ORDER BY 乡镇街道,
         @t1s地址,
          身份证号',
                         '',
                         2,
                         1,
                         1200
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         16,
                         '贫困户补贴判断',
                         'select 类型,乡镇街道,身份证号,@t1s姓名,@t1s地址,领取时间,补贴金额,备注 from 

(
SELECT ''@aimtype'' AS 类型,
       @tablepre1.Region AS 乡镇街道,
       @tablepre1.ID AS 身份证号,
       group_concat(DISTINCT @tablepre1.Name) AS @t1s姓名,
       group_concat(DISTINCT @tablepre1.Addr) AS @t1s地址,
       @table2.DataDate AS 领取时间,
       sum(@table2.Amount) AS 补贴金额,
       '''' AS 备注
  FROM    @tablepre1
       JOIN
       @table2 ON @tablepre1.id = @table2.id
 WHERE  
        length( @tablepre1.ID )>1
group by @table2.id)
where 补贴金额 >= @para',
                         'SELECT DISTINCT 类型,
                乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址 ,                
                group_concat( 备注, ''，'') as 备注
  FROM @tablename
group by 类型,
                乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址',
                         'SELECT DISTINCT 类型,                
                group_concat(distinct 乡镇街道) 乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) @t1s姓名,      
                @t1s地址,
                领取时间,
                group_concat( 备注, ''，'') as 备注 @injection 
  FROM @tablename
group by 类型,
                乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址',
                         '',
                         5,
                         1,
                         1300
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         17,
                         '一表数据在另一表不存在',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.Region AS @t1s乡镇街道,
       @table1.ID AS 身份证号,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,       
       @table1.sDataDate AS @t1s时间 
  FROM @table1
 WHERE (
           SELECT count(1) AS num
             FROM @tablepre2
            WHERE @table1.ID = @tablepre2.ID
       ) =  0 and (length(@table1.ID) > 0)',
                         'SELECT DISTINCT 类型,
                group_concat(distinct @t1s乡镇街道) @t1s乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) @t1s姓名,
                group_concat(distinct @t1s地址)  @t1s地址,   
                 count(@t1s时间) 发放次数,
                '''' as 备注
  FROM @tablename
group by 类型,           
                身份证号
              ',
                         'SELECT DISTINCT 类型,                
                group_concat(distinct @t1s乡镇街道) @t1s乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) @t1s姓名,      
                @t1s地址,
                @t1s时间,
                ''''  备注 @injection 
  FROM @tablename
group by 类型,
                @t1s乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址',
                         '',
                         2,
                         1,
                         1400
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         18,
                         '重复领取数据',
                         'SELECT  ''@aimtype'' AS 类型,
       @table1.Region as @t1s乡镇街道,
       @table1.ID as 身份证号,
       group_concat(DISTINCT @table1.Name) AS @t1s姓名,
       group_concat(DISTINCT @table1.Addr) AS @t1s地址,
       count(@table1.DataDate) AS @t1s次数,
        group_concat(@table1.Area)  备注
  FROM @table1
 WHERE  length( @table1.ID )>1
 GROUP BY id
HAVING count(id) > @para
',
                         'SELECT DISTINCT 类型,
                @t1s乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址 ,   
                 @t1s次数,
                备注 as 备注
  FROM @tablename
group by 类型,
                @t1s乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址
order by @t1s乡镇街道,@t1s次数,@t1s地址 ,  身份证号',
                         'SELECT DISTINCT 类型,                
                group_concat(distinct @t1s乡镇街道) @t1s乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) @t1s姓名,      
                @t1s地址,
                @t1s次数,
                备注 备注 @injection 
  FROM @tablename
group by 类型,
                @t1s乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址
order by @t1s乡镇街道,@t1s次数,@t1s地址 ,  身份证号',
                         '',
                         2,
                         1,
                         1500
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         19,
                         '双表查询相同数据',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.Region AS @t1s乡镇街道,
       @table1.ID AS 身份证号,
       @table2.Name AS @t2s姓名,
       @table2.Addr AS @t2s地址,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,
       @table1.sDataDate AS @t1s时间
  FROM @table1
       JOIN
      @table2 ON @table1.ID = @table2.ID
 WHERE   length( @table1.ID )>1  
 ORDER BY @t1s乡镇街道,@t1s地址,
          身份证号,@t1s时间 ',
                         'SELECT distinct 类型,
       group_concat(distinct @t1s乡镇街道) @t1s乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,       
       group_concat(distinct @t2s姓名) @t2s姓名,  
        @t1s地址,
       min(date(@t1s时间) ) 开始时间,     
       max(date(@t1s时间) ) 最后时间,
       '''' 金额,       
       group_concat(distinct @t2s地址) 备注       
  FROM @tablename
 GROUP BY 类型,身份证号           
 ORDER BY @t1s乡镇街道,@t1s地址,身份证号',
                         'SELECT distinct 类型,
       group_concat(distinct @t1s乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,
       @t1s地址,  
       min(@t1s时间) ||'' 至 '' || max(@t1s时间) 领取时间,
      ''@t2s地址:''  || ifnull(group_concat(distinct @t2s地址), '''') 备注 @injection  
  FROM @tablename
 GROUP BY 类型,         
          
          身份证号           
 ORDER BY 乡镇街道,@t1s地址,身份证号',
                         '',
                         2,
                         1,
                         1600
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         20,
                         '面积超出',
                         'SELECT ''@aimtype'' AS 类型,    
       @table1.Region AS @t1s乡镇街道,
       @table1.ID AS 身份证号,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,       
       @table1.sDataDate AS @t1s时间,    
       @table1.Amount as 家庭人口, 
@table1.Area as 面积,  
       @table1.Area/@table1.Amount as 平均面积
  FROM @table1
  where @table1.Amount > 0 and 
     length(@table1.ID) > 1 @para
order by @t1s乡镇街道,@t1s地址,   身份证号 ',
                         'SELECT distinct 类型,
       group_concat(distinct  @t1s乡镇街道) @t1s乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,  
        @t1s地址,
        date(@t1s时间) 开始时间,     
       '''' 最后时间,
       家庭人口 人口,       
       ''平均面积:''|平均面积 备注       
  FROM @tablename
 GROUP BY 类型,身份证号           
 ORDER BY  @t1s乡镇街道,@t1s地址,身份证号',
                         'SELECT distinct 类型,
       group_concat(distinct @t1s乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,
       @t1s地址,  
       max(@t1s时间) 领取时间,
       ''平均面积:'' ||平均面积 || '',人口：'' ||  家庭人口 || '',面积:'' || 面积  备注 @injection  
  FROM @tablename
 GROUP BY 类型,  
          身份证号           
 ORDER BY 乡镇街道,@t1s地址,身份证号',
                         '',
                         2,
                         1,
                         1700
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         21,
                         '身份证年龄判断',
                         'SELECT DISTINCT ''@aimtype'' AS 类型,
                @table1.Region AS 乡镇街道,
                @table1.ID AS 身份证号,
                @table1.Addr AS @t1s地址,
                @table1.Name AS @t1s姓名,
                @table1.sDataDate AS @t1s领取时间,
                @table1.Amount AS @t1s领取金额,
                datadate - datetime(substr(id, 7, 4) || ''-'' || substr(id, 11, 2) || ''-'' || substr(id, 13, 2) ) AS 年龄,
                @table1.Type as 备注
  FROM @table1
 WHERE   @para',
                         'SELECT distinct 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,     
        @t1s地址,
       group_concat(@t1s领取时间) @t1s领取时间,            
       sum(@t1s领取金额) 金额,       
       ''年龄:''||年龄|| '',''|| 备注 as 备注       
  FROM @tablename
 GROUP BY 类型,身份证号           
 ORDER BY 乡镇街道,@t1s地址,身份证号, @t1s领取时间,备注 
',
                         'SELECT distinct 类型,
       group_concat(distinct 乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,
       @t1s地址,  
      group_concat(@t1s领取时间) 领取时间,
       ''金额:'' ||@t1s领取金额|| '',年龄:'' ||  年龄  备注 @injection  
  FROM @tablename
 GROUP BY 类型,  
          身份证号           
 ORDER BY 乡镇街道,@t1s地址,身份证号,备注',
                         '',
                         2,
                         1,
                         1800
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         22,
                         '金额超出',
                         '
SELECT ''@aimtype'' AS 类型,
       @table1.Region AS @t1s乡镇街道,
       @table1.ID AS 身份证号,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,
       @table1.sDataDate AS @t1s时间,
       @table1.Amount AS 金额         
  FROM @table1 　
where  length( @table1.ID )>1
 GROUP BY @table1.Amount, strftime(''%Y'', @table1.sDataDate) 
HAVING (sum(@table1.Amount)  @para ) 
 ORDER BY @t1s乡镇街道, @t1s地址,身份证号',
                         'SELECT distinct 类型,
       group_concat(distinct @t1s乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,  
       group_concat(distinct @t2s姓名) @t2s姓名,
       @t1s地址,
       min(date(@t1s时间) ) 开始领取时间,     
       max(date(@t1s时间) ) 最后领取时间,
       sum(金额) 金额,
       '''' 备注     
  FROM @tablename
 GROUP BY 类型,身份证号, @t1s地址,备注
 ORDER BY 乡镇街道,@t1s地址,身份证号',
                         'SELECT distinct 类型,
       group_concat(distinct @t1s乡镇街道) 乡镇街道,
       身份证号,
       group_concat(distinct @t1s姓名) @t1s姓名,
       @t1s地址,  
       ''开始领取时间:'' || min(date(@t1s时间) )  ||''最后领取时间:''  ||   
       max(date(@t1s时间) )   领取时间,
       ''金额:'' || sum(金额)  备注 @injection  
  FROM @tablename
 GROUP BY 类型,  @t1s地址,  
          身份证号           
 ORDER BY 乡镇街道,@t1s地址,身份证号',
                         '',
                         2,
                         1,
                         1900
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         23,
                         '扶贫信贷商业情况',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.Region AS @t1s乡镇街道,
      @table1.ID AS 身份证号,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,
       @table1.sDataDate AS @t1s时间,
       @table1.Amount AS 金额
  FROM @table1
 WHERE (
           SELECT count(1) AS num
             FROM tbCompareRegister
            WHERE @table1.ID = tbCompareRegister.ID
       )
=      0 AND 
       (
           SELECT count(1) AS num
             FROM tbComparemachineInfo
            WHERE @table1.ID = tbComparemachineInfo.ID
       )
=      0 AND 
       (
           SELECT count(1) AS num
             FROM tbCompareCompanyInfo
            WHERE @table1.ID = tbCompareCompanyInfo.ID
       )
=      0
and length( @table1.ID )>1',
                         'SELECT DISTINCT 类型,
                @t1s乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) @t1s姓名,      
                group_concat(distinct @t1s地址 ) 地址,  
                sum(金额)             
  FROM @tablename
group by 类型,
                @t1s乡镇街道,
                身份证号 
               
order by @t1s乡镇街道, 地址 ,  身份证号',
                         'SELECT DISTINCT 类型,                
                group_concat(distinct @t1s乡镇街道) @t1s乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) @t1s姓名,      
                 group_concat(distinct @t1s地址 ) 地址,  
                ''''       时间,
                sum(金额) as 备注 @injection 
  FROM @tablename
group by 类型,
                @t1s乡镇街道,
                身份证号,
                @t1s姓名,
                @t1s地址',
                         '',
                         2,
                         1,
                         2000
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         24,
                         '同表双时间判断',
                         'SELECT ''@aimtype'' AS 类型,
       tbSourceMedicalAd.Region AS 乡镇街道,
       tbSourceMedicalAd.ID AS  身份证号,
tbSourceMedicalAd.Name AS  @t1s姓名,
       tbSourceMedicalAd.Addr AS @t1s地址,
       tbSourceMedicalAd.sDataDate AS 时间,
       tbSourceMedicalAd.RelateID AS 医生身份证号,
       tbSourceMedicalAd.RelateName AS 医生姓名,
       tbCompareInHospital.DataDate AS 入院时间,
       tbCompareInHospital.DataDate1 AS 出院时间
  FROM tbSourceMedicalAd
       JOIN
       tbCompareInHospital ON tbSourceMedicalAd.id = tbCompareInHospital.id
 WHERE tbSourceMedicalAd.sDataDate BETWEEN datetime(tbCompareInHospital.DataDate) AND datetime(tbCompareInHospital.dataDate1)
and length(tbSourceMedicalAd.ID) > 1',
                         'SELECT DISTINCT 类型,
                乡镇街道,
                身份证号,
                group_concat(DISTINCT 公共卫生姓名) 公共卫生姓名,
                group_concat(DISTINCT 公共卫生地址) 地址,
                时间 服务时间,
                医生姓名,
                医生身份证号,
                入院时间,
                出院时间
  FROM @tablename
group by  
                身份证号    
order by 乡镇街道,  身份证号',
                         'SELECT DISTINCT 类型,
                乡镇街道,
                身份证号,
                group_concat(DISTINCT 公共卫生姓名) 公共卫生姓名,
                group_concat(DISTINCT 公共卫生地址) 地址,
                时间 时间,
                ''医生身份证号：'' || 医生身份证号 || '' ，医生姓名：'' || 医生姓名 || ''入院时间:'' || 入院时间 || ''-出院时间:'' || 出院时间 AS 备注  @injection 
  FROM @tablename
 GROUP BY 身份证号
 ORDER BY 乡镇街道,
          地址,
          身份证号',
                         '',
                         1,
                         1,
                         2100
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         25,
                         '一表存在一表不存在-2',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.Region AS @t1s乡镇街道,
       @table1.ID AS 身份证号,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,       
       @table1.sDataDate AS 发放时间,       
       @table1.Amount as 发放金额
  FROM @table1
 WHERE (
           SELECT count(1) AS num
             FROM @table2
            WHERE @table1.ID = @table2.ID
       ) =  0 and (length(@table1.ID) > 0)',
                         'SELECT DISTINCT 类型,
                group_concat(distinct @t1s乡镇街道) @t1s乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) @t1s姓名,
                group_concat(distinct @t1s地址)  @t1s地址,   
                 count(发放时间) 发放次数,
                ''金额: '' ||  发放金额 as 备注
  FROM @tablename
group by 类型,                
                身份证号
order by @t1s乡镇街道 
               ',
                         'SELECT DISTINCT 类型,                
                group_concat(distinct @t1s乡镇街道) @t1s乡镇街道,
                身份证号,
                group_concat(distinct @t1s姓名) @t1s姓名,      
                @t1s地址,
                min(发放时间) || '' 至 '' || max(发放时间) 领取时间,
                ''金额:'' || sum(发放金额) as 备注 @injection 
  FROM @tablename
group by 类型,               
                身份证号,                
                @t1s地址
order by @t1s乡镇街道',
                         '',
                         5,
                         1,
                         1450
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         26,
                         '按年重复数据',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.Region 乡镇街道,
       @table1.ID 身份证号,
       @table1.Name 姓名,
       @table1.Addr 地址,
       @table1.Type 公司,
       @table1.sDataDate 时间,
       b.DataDateYear 年度
  FROM @table1
 join (
           SELECT id,  strftime(''%Y'',sDataDate) DataDateYear
             FROM @table1              
            GROUP BY DataDateYear , ID
           HAVING count(id) >= @para
       ) b on @table1.ID = b.ID 
       where b.datadateyear =  strftime(''%Y'',@table1.sDataDate)
and  length(@table1.id) > 1   
 ORDER BY b.ID, b.DataDateYear;
',
                         'SELECT DISTINCT 类型,
                乡镇街道,
                身份证号,
                group_concat(distinct 姓名) 姓名,      
                group_concat(distinct 地址 ) 地址,  
                count(时间)    次数,
                年度,
                  group_concat(distinct        公司) 公司
  FROM @tablename
group by 类型,
                 
                身份证号   
order by 乡镇街道, 地址 ,  身份证号',
                         'SELECT DISTINCT 类型,
                乡镇街道,
                身份证号,
                group_concat(distinct 姓名) 姓名,      
                group_concat(distinct 地址 ) 地址,  
               年度 时间,
               ''次数:'' || count(身份证号)  as 备注  @injection 
  FROM @tablename
group by 类型,
                 
                身份证号 
order by 乡镇街道, 地址 ,  身份证号',
                         '',
                         2,
                         1,
                         9999999
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         27,
                         '整村推进项目中标项目排序',
                         'SELECT ''@aimtype'' AS 类型,
                  tbSourceVillageAdv.serial2 营业执照码,
                  tbSourceVillageAdv.relatename 公司名称,
                  tbSourceVillageAdv.region 乡镇街道,
                  tbSourceVillageAdv.number 项目编号,
                  tbSourceVillageAdv.name 项目名称,
                  tbSourceVillageAdv.addr 项目地址,
                  tbSourceVillageAdv.serial1 组织机构码,
                  tbSourceVillageAdv.Relation 统一信用码
             FROM tbSourceVillageAdv
                  JOIN
                  (
                      SELECT serial1,
                             relatename,
                             count() 
                        FROM tbSourceVillageAdv
                       GROUP BY serial1
                       ORDER BY count() DESC
                       LIMIT 20
                  )
                  b ON b.serial1 = tbSourceVillageAdv.Serial1
            ORDER BY 乡镇街道,tbSourceVillageAdv.serial1 DESC',
                         'SELECT 类型,
       营业执照码,
       组织机构码,
       统一信用码,
       公司名称,
       count(项目名称) 中标数量   
  FROM @tablename
GROUP BY 组织机构码          
 ORDER BY 中标数量 desc',
                         'SELECT DISTINCT 类型,
                乡镇街道,
                组织机构码 as 组织机构码_营业执照_统一信用码,
                公司名称,
                '''' 地址,
                '''' 时间,
                ''中标次数:'' || count(项目名称) AS 备注  @injection 
  FROM @tablename
GROUP BY 类型,
          组织机构码_营业执照_统一信用码
 ORDER BY count(项目名称) desc;',
                         '',
                         2,
                         1,
                         9999999
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         28,
                         '整村推进项目中标项目排序(按乡镇)',
                         'SELECT 
       ''@aimtype'' as 类型,
       tbSourceVillageAdv.serial2 营业执照码,
       tbSourceVillageAdv.relatename 公司名称,
       tbSourceVillageAdv.region 乡镇街道,
       tbSourceVillageAdv.number 项目编号,
       tbSourceVillageAdv.name 项目名称,
       tbSourceVillageAdv.addr 项目地址,
       tbSourceVillageAdv.serial1 组织机构码,
       tbSourceVillageAdv.Relation 统一信用码
  FROM tbSourceVillageAdv
 WHERE serial2 IN (
           SELECT serial2
             FROM tbSourceVillageAdv
            GROUP BY serial2,region
            ORDER BY count() DESC
            LIMIT @para
       )
 ORDER BY tbSourceVillageAdv.serial2 DESC,
          乡镇街道',
                         'SELECT 类型, 乡镇街道, 营业执照码,
       公司名称,
       count(项目名称) 中标数量
  FROM @tablename
GROUP BY 营业执照码,乡镇街道,
          公司名称
 ORDER BY 中标数量 desc',
                         'SELECT DISTINCT 类型,
                乡镇街道,
                营业执照码 身份证号,
                公司名称 姓名,      
                '''' 地址,  
               '''' 时间,
               ''中标次数:'' || count(项目名称)  as 备注  @injection 
  FROM @tablename
group by 类型,
                乡镇街道,
                身份证号 
order by 身份证号,乡镇街道',
                         '',
                         2,
                         1,
                         9999999
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         29,
                         '家庭经济情况',
                         'SELECT ''@aimtype'' AS 类型,
       @table1.Region AS 乡镇街道,
       @table1.ID AS 身份证号,
       @table1.Name AS @t1s姓名,
       @table1.Addr AS @t1s地址,
       @table1.sDataDate AS 领取时间,
       @table1.Amount AS 领取金额,
       a.备注
  FROM (
           SELECT 身份证号,
                  ''商品房数量:'' || sum(数量) || '','' || group_concat(备注) AS 备注
             FROM (
                      SELECT DISTINCT @table1.ID AS 身份证号,
                                      b.数量,
                                      '' 编号:'' || b.型号 AS 备注
                        FROM @table1,
                             (
                                 SELECT DISTINCT ID,
                                                 Name,
                                                 group_concat(Type, ''/'') AS 型号,
                                                 count() 数量
                                   FROM @table2
                                  WHERE length(ID) > 1
                                  GROUP BY ID,
                                           Name
                             )
                             b
                       WHERE length(@table1.ID) > 1 AND 
                             b.ID = @table1.ID
                       GROUP BY 身份证号
                      UNION ALL
                      SELECT @table1.ID AS 身份证号,
                             b.数量,
                             ''编号:'' || b.备注 || '',亲属关系:'' || @table3.Relation || '',亲属ID:'' || @table3.RelateID AS 备注
                        FROM @table1,
                             @table3,
                             (
                                 SELECT ID,
                                        Name,
                                        group_concat(Type, ''/'') AS 备注,
                                        count() 数量
                                   FROM @table2
                                  WHERE length(ID) > 1
                                  GROUP BY ID,
                                           Name
                             )
                             b
                       WHERE @table3.sRelateID = @table1.ID AND 
                             (@table3.Relation IN (''夫'', ''妻'', ''配偶'', ''子'', ''女'', ''长子'', ''长女'', ''二子'', ''二女'', ''次子'', ''次女'', ''父'', ''母'', ''父母'') ) AND 
                             @table3.ID = b.ID AND 
                             length(@table1.ID) > 1
                       GROUP BY 身份证号
                  )
            GROUP BY 身份证号
           HAVING sum(数量) > @para
       )
       a
       JOIN
       @table1 ON a.身份证号 = @table1.id
 WHERE length(@table1.id) > 1
 ORDER BY @table1.Region,
          @table1.id;',
                         'SELECT DISTINCT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名,
      min(date(领取时间) ) 开始领取时间,     
      max(date(领取时间) ) 最后领取时间,
      sum(领取金额) 金额,
      group_concat(distinct 备注) 备注
  FROM @tablename
 GROUP BY 类型,          
          身份证号
 ORDER BY 乡镇街道,@t1s地址,
          身份证号
',
                         'SELECT DISTINCT 类型,
      group_concat(distinct 乡镇街道) 乡镇街道,
      身份证号,
      group_concat(distinct @t1s姓名) @t1s姓名,  
      @t1s地址,    
      min(领取时间) ||'' 至 '' || max(领取时间) 领取时间,  
     ''领取金额:'' || sum(领取金额) || '','' || 备注  @injection 
  FROM @tablename
 GROUP BY 类型,          
          身份证号
 ORDER BY 乡镇街道,@t1s地址,
          身份证号',
                         '',
                         3,
                         1,
                         9999999
                     );

INSERT INTO RulesTmp (
                         RowID,
                         TmpName,
                         Rules,
                         Rule2,
                         Rule3,
                         PreRules,
                         Type,
                         Status,
                         Seq
                     )
                     VALUES (
                         30,
                         '时间异常',
                         'SELECT  ''@aimtype'' as 类型,
       @table1.Region 乡镇街道,
       @table1.ID 身份证号,
       @table1.Name 姓名,
       @table1.Addr 地址,
       @table1.type 公司,
       datadate1,
       datadate,
       julianday(datadate1) - julianday(sdatadate) 培训时间
  FROM @table1
 WHERE 培训时间 < 7 OR 
       培训时间 > 365',
                         'SELECT  ''@aimtype'' as 类型,
       @table1.Region 乡镇街道,
       @table1.ID 身份证号,
       @table1.Name 姓名,
       @table1.Addr 地址,
       @table1.type 公司,
      培训时间
from @tablename
order by 公司,身份证号',
                         'SELECT    类型,
         乡镇街道,
        身份证号,
         姓名,
         地址,
      
      培训时间,
     ''公司:'' || @table1.type as 备注  @injection 
from @tablename
order by 公司,身份证号',
                         '',
                         2,
                         1,
                         9999999
                     );


-- 表：DataLabel
DROP TABLE IF EXISTS DataLabel;

CREATE TABLE DataLabel (
    RowID    INTEGER      PRIMARY KEY,
    ParentID INTEGER,
    Label    VARCHAR (20) NOT NULL,
    Col1     VARCHAR (20),
    Seq      INTEGER      NOT NULL
                          DEFAULT (999999),
    UNIQUE (
        RowID
    )
);

INSERT INTO DataLabel (
                          RowID,
                          ParentID,
                          Label,
                          Col1,
                          Seq
                      )
                      VALUES (
                          1,
                          NULL,
                          '2015',
                          NULL,
                          999999
                      );

INSERT INTO DataLabel (
                          RowID,
                          ParentID,
                          Label,
                          Col1,
                          Seq
                      )
                      VALUES (
                          2,
                          NULL,
                          '2014',
                          NULL,
                          999999
                      );


-- 表：Setting
DROP TABLE IF EXISTS Setting;

CREATE TABLE Setting (
    ID           INTEGER      PRIMARY KEY AUTOINCREMENT,
    SettingName  VARCHAR (10) NOT NULL,
    SettingValue INTEGER      NOT NULL
                              DEFAULT (0),
    Status       INTEGER      DEFAULT (1) 
);

INSERT INTO Setting (
                        ID,
                        SettingName,
                        SettingValue,
                        Status
                    )
                    VALUES (
                        1,
                        'ImportDataChecking',
                        1,
                        1
                    );


-- 表：User
DROP TABLE IF EXISTS User;

CREATE TABLE User (
    RowID    INTEGER      PRIMARY KEY AUTOINCREMENT
                          NOT NULL,
    Username VARCHAR (10) NOT NULL,
    Password VARCHAR (20),
    IsFirst  INTEGER      DEFAULT 1,
    Status   INTEGER      DEFAULT 1,
    RegionID INTEGER
);

INSERT INTO User (
                     RowID,
                     Username,
                     Password,
                     IsFirst,
                     Status,
                     RegionID
                 )
                 VALUES (
                     1,
                     'admin',
                     '1',
                     0,
                     1,
                     139
                 );

INSERT INTO User (
                     RowID,
                     Username,
                     Password,
                     IsFirst,
                     Status,
                     RegionID
                 )
                 VALUES (
                     2,
                     'nz0001',
                     '1',
                     0,
                     1,
                     58
                 );

INSERT INTO User (
                     RowID,
                     Username,
                     Password,
                     IsFirst,
                     Status,
                     RegionID
                 )
                 VALUES (
                     3,
                     'es000103',
                     '1',
                     0,
                     1,
                     126
                 );


-- 表：Task
DROP TABLE IF EXISTS Task;

CREATE TABLE Task (
    RowID       INTEGER       PRIMARY KEY AUTOINCREMENT,
    TaskName    VARCHAR (50)  NOT NULL,
    CreateDate  DATETIME (0)  NOT NULL,
    TaskComment VARCHAR (100),
    Region      VARCHAR (50)  NOT NULL,
    DBInfo      VARCHAR (20)  NOT NULL,
    Enable      INTEGER       DEFAULT (0),
    Status      INTEGER       DEFAULT (1),
    TStatus     INTEGER       DEFAULT (0),
    UserName    VARCHAR (50) 
);

INSERT INTO Task (
                     RowID,
                     TaskName,
                     CreateDate,
                     TaskComment,
                     Region,
                     DBInfo,
                     Enable,
                     Status,
                     TStatus,
                     UserName
                 )
                 VALUES (
                     115,
                     '第1次大数据比对',
                     '2017-02-22 12:41:04',
                     '湖北省2017年02月22日12点41分03秒，进行精准扶贫政策落实情况检查大数据分析比对。',
                     '',
                     '67696864588.4927',
                     0,
                     1,
                     0,
                     'admin'
                 );

INSERT INTO Task (
                     RowID,
                     TaskName,
                     CreateDate,
                     TaskComment,
                     Region,
                     DBInfo,
                     Enable,
                     Status,
                     TStatus,
                     UserName
                 )
                 VALUES (
                     116,
                     '第2次大数据比对',
                     '2017-02-22 13:02:26',
                     '湖北省2017年02月22日13点02分25秒，进行精准扶贫政策落实情况检查大数据分析比对。',
                     '',
                     '67698146955.029',
                     0,
                     10,
                     0,
                     'admin'
                 );


-- 表：HB_Region
DROP TABLE IF EXISTS HB_Region;

CREATE TABLE HB_Region (
    RowID      INTEGER      PRIMARY KEY AUTOINCREMENT,
    ParentID   INTEGER,
    RegionCode CHAR (10),
    RegionName CHAR (25),
    Level      INTEGER,
    IP         VARCHAR (50),
    Seq        INTEGER,
    Status     INTEGER      DEFAULT (1) 
);

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          1,
-                         1,
                          'HB',
                          '湖北省',
                          0,
                          NULL,
                          0,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          2,
                          1,
                          'HBQJ',
                          '潜江市',
                          1,
                          NULL,
                          2300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          3,
                          1,
                          'HBWH',
                          '武汉市',
                          1,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          4,
                          1,
                          'HBHS',
                          '黄石市',
                          1,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          5,
                          1,
                          'HBSY',
                          '十堰市',
                          1,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          6,
                          1,
                          'HBXY',
                          '襄阳市',
                          1,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          7,
                          1,
                          'HBYC',
                          '宜昌市',
                          1,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          8,
                          1,
                          'HBJZ',
                          '荆州市',
                          1,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          9,
                          1,
                          'HBJM',
                          '荆门市',
                          1,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          10,
                          1,
                          'HBEZ',
                          '鄂州市',
                          1,
                          NULL,
                          1700,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          11,
                          1,
                          'HBXG',
                          '孝感市',
                          1,
                          NULL,
                          1800,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          12,
                          1,
                          'HBHG',
                          '黄冈市',
                          1,
                          NULL,
                          1900,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          13,
                          1,
                          'HBXN',
                          '咸宁市',
                          1,
                          NULL,
                          2000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          14,
                          1,
                          'HBSZ',
                          '随州市',
                          1,
                          NULL,
                          2100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          15,
                          1,
                          'HBES',
                          '恩施土家族苗族自治州',
                          1,
                          NULL,
                          2200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          16,
                          1,
                          'HBSLJ',
                          '神农架林区',
                          1,
                          NULL,
                          2600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          17,
                          1,
                          'HBTM',
                          '天门市',
                          1,
                          NULL,
                          2400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          18,
                          1,
                          'HBXT',
                          '仙桃市',
                          1,
                          NULL,
                          2500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          19,
                          0,
                          '',
                          '',
                          1,
                          NULL,
                          999999,
                          0
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          20,
                          0,
                          '',
                          '',
                          1,
                          NULL,
                          999999,
                          0
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          21,
                          3,
                          'JA',
                          '江岸区',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          22,
                          3,
                          'JH',
                          '江汉区',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          23,
                          3,
                          'QK',
                          '硚口区',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          24,
                          3,
                          'HY',
                          '汉阳区',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          25,
                          3,
                          'WC',
                          '武昌区',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          26,
                          3,
                          'QS',
                          '青山区',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          27,
                          3,
                          'HS',
                          '洪山区',
                          2,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          28,
                          3,
                          'CD',
                          '蔡甸区',
                          2,
                          NULL,
                          1700,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          29,
                          3,
                          'JX',
                          '江夏区',
                          2,
                          NULL,
                          1800,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          30,
                          3,
                          'DXH',
                          '东西湖区',
                          2,
                          NULL,
                          1900,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          31,
                          3,
                          'HN',
                          '汉南区',
                          2,
                          NULL,
                          2000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          32,
                          3,
                          'HP',
                          '黄陂区',
                          2,
                          NULL,
                          2100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          33,
                          3,
                          'XZ',
                          '新洲区',
                          2,
                          NULL,
                          2200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          34,
                          3,
                          'DHGX',
                          '东湖高新区',
                          2,
                          NULL,
                          2300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          35,
                          3,
                          'JJKF',
                          '经济开发区',
                          2,
                          NULL,
                          2400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          36,
                          3,
                          'DHFJ',
                          '东湖风景区',
                          2,
                          NULL,
                          2500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          37,
                          3,
                          'WHHG',
                          '武汉化工区',
                          2,
                          NULL,
                          2600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          38,
                          4,
                          'DY',
                          '大冶市',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          39,
                          4,
                          'YX',
                          '阳新县',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          40,
                          4,
                          'HSG',
                          '黄石港区',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          41,
                          4,
                          'XSS',
                          '西塞山区',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          42,
                          4,
                          'XL',
                          '下陆区',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          43,
                          4,
                          'TS',
                          '铁山区',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          44,
                          4,
                          'JJJS',
                          '经济技术开发区',
                          2,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          45,
                          4,
                          'XGWL',
                          '新港物流工业园区',
                          2,
                          NULL,
                          1700,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          46,
                          5,
                          'DJK',
                          '丹江口市',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          47,
                          5,
                          'YY',
                          '郧阳区',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          48,
                          5,
                          'YX',
                          '郧西县',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          49,
                          5,
                          'FX',
                          '房县',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          50,
                          5,
                          'ZS',
                          '竹山县',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          51,
                          5,
                          'ZX',
                          '竹溪县',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          52,
                          5,
                          'MJ',
                          '茅箭区',
                          2,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          53,
                          5,
                          'ZW',
                          '张湾区',
                          2,
                          NULL,
                          1700,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          54,
                          5,
                          'WDS',
                          '武当山特区',
                          2,
                          NULL,
                          1800,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          55,
                          5,
                          'JJJS',
                          '经济技术开发区',
                          2,
                          NULL,
                          1900,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          56,
                          6,
                          'DJK',
                          '枣阳市',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          57,
                          6,
                          'YY',
                          '宜城市',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          58,
                          6,
                          'YX',
                          '南漳县',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          59,
                          6,
                          'FX',
                          '保康县',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          60,
                          6,
                          'ZS',
                          '谷城县',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          61,
                          6,
                          'ZX',
                          '老河口市',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          62,
                          6,
                          'MJ',
                          '襄城区',
                          2,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          63,
                          6,
                          'ZW',
                          '襄州区',
                          2,
                          NULL,
                          1700,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          64,
                          6,
                          'WDS',
                          '樊城区',
                          2,
                          NULL,
                          1800,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          65,
                          6,
                          'JJJS',
                          '高新区',
                          2,
                          NULL,
                          1900,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          66,
                          6,
                          'JJJS',
                          '经开区',
                          2,
                          NULL,
                          2000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          67,
                          6,
                          'JJJS',
                          '鱼梁洲经济开发区',
                          2,
                          NULL,
                          2100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          68,
                          7,
                          'DJK',
                          '宜都市',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          69,
                          7,
                          'YY',
                          '枝江市',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          70,
                          7,
                          'YX',
                          '当阳市',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          71,
                          7,
                          'FX',
                          '远安县',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          72,
                          7,
                          'ZS',
                          '兴山县',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          73,
                          7,
                          'ZX',
                          '秭归县',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          74,
                          7,
                          'MJ',
                          '长阳县',
                          2,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          75,
                          7,
                          'ZW',
                          '五峰土家族自治县',
                          2,
                          NULL,
                          1700,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          76,
                          7,
                          'WDS',
                          '夷陵区',
                          2,
                          NULL,
                          1800,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          77,
                          7,
                          'JJJS',
                          '西陵区',
                          2,
                          NULL,
                          1900,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          78,
                          7,
                          'JJJS',
                          '伍家岗区',
                          2,
                          NULL,
                          2000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          79,
                          7,
                          'JJJS',
                          '点军区',
                          2,
                          NULL,
                          2100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          80,
                          7,
                          'JJJS',
                          '猇亭区',
                          2,
                          NULL,
                          2200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          81,
                          8,
                          'ZX',
                          '石首市',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          82,
                          8,
                          'DJK',
                          '荆州区',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          83,
                          8,
                          'YY',
                          '沙市区',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          84,
                          8,
                          'YX',
                          '江陵县',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          85,
                          8,
                          'FX',
                          '松滋市',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          86,
                          8,
                          'ZS',
                          '公安县',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          87,
                          8,
                          'MJ',
                          '监利县',
                          2,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          88,
                          8,
                          'ZW',
                          '洪湖市',
                          2,
                          NULL,
                          1700,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          89,
                          9,
                          'DJK',
                          '京山县',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          90,
                          9,
                          'YY',
                          '沙洋县',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          91,
                          9,
                          'YX',
                          '钟祥市',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          92,
                          9,
                          'FX',
                          '东宝区',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          93,
                          9,
                          'ZS',
                          '掇刀区',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          94,
                          8,
                          'ZX',
                          '石首市',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          95,
                          10,
                          'YY',
                          '华容区',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          96,
                          10,
                          'DJK',
                          '鄂城区',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          97,
                          10,
                          'YX',
                          '梁子湖区',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          98,
                          11,
                          'DJK',
                          '孝南区',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          99,
                          11,
                          'YY',
                          '汉川市',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          100,
                          11,
                          'YX',
                          '应城市',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          101,
                          11,
                          'FX',
                          '云梦县',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          102,
                          11,
                          'ZS',
                          '安陆市',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          103,
                          11,
                          'ZX',
                          '大悟县',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          104,
                          11,
                          'MJ',
                          '孝昌县',
                          2,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          105,
                          12,
                          'DJK',
                          '黄州区',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          106,
                          12,
                          'YY',
                          '团风县',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          107,
                          12,
                          'YX',
                          '红安县',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          108,
                          12,
                          'FX',
                          '麻城市',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          109,
                          12,
                          'ZS',
                          '罗田县',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          110,
                          12,
                          'ZX',
                          '英山县',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          111,
                          12,
                          'MJ',
                          '浠水县',
                          2,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          112,
                          12,
                          'ZW',
                          '蕲春县',
                          2,
                          NULL,
                          1700,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          113,
                          12,
                          'WDS',
                          '黄梅县',
                          2,
                          NULL,
                          1800,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          114,
                          12,
                          'JJJS',
                          '龙感湖管理区',
                          2,
                          NULL,
                          1900,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          115,
                          13,
                          'DJK',
                          '咸安区',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          116,
                          13,
                          'YY',
                          '嘉鱼县',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          117,
                          13,
                          'YX',
                          '赤壁市',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          118,
                          13,
                          'FX',
                          '通城县',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          119,
                          13,
                          'ZS',
                          '崇阳县',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          120,
                          13,
                          'ZX',
                          '通山县',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          121,
                          14,
                          'DJK',
                          '随县',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          122,
                          14,
                          'YY',
                          '广水市',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          123,
                          14,
                          'YX',
                          '曾都区',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          124,
                          15,
                          'DJK',
                          '恩施市',
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          125,
                          15,
                          'YY',
                          '利川市',
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          126,
                          15,
                          'YX',
                          '建始县',
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          127,
                          15,
                          'FX',
                          '巴东县',
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          128,
                          15,
                          'ZS',
                          '宣恩县',
                          2,
                          NULL,
                          1400,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          129,
                          15,
                          'ZX',
                          '来凤县',
                          2,
                          NULL,
                          1500,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          130,
                          15,
                          'MJ',
                          '咸丰县',
                          2,
                          NULL,
                          1600,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          131,
                          15,
                          'ZW',
                          '鹤峰县',
                          2,
                          NULL,
                          1700,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          135,
                          17,
                          'FX',
                          NULL,
                          2,
                          NULL,
                          1300,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          136,
                          2,
                          'DJK',
                          NULL,
                          2,
                          NULL,
                          1000,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          137,
                          16,
                          'YY',
                          NULL,
                          2,
                          NULL,
                          1100,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          138,
                          18,
                          'YX',
                          NULL,
                          2,
                          NULL,
                          1200,
                          1
                      );

INSERT INTO HB_Region (
                          RowID,
                          ParentID,
                          RegionCode,
                          RegionName,
                          Level,
                          IP,
                          Seq,
                          Status
                      )
                      VALUES (
                          139,
                          1,
                          'HB',
                          '',
                          1,
                          NULL,
                          1,
                          1
                      );


-- 表：CompareAim
DROP TABLE IF EXISTS CompareAim;

CREATE TABLE CompareAim (
    RowID      INTEGER       PRIMARY KEY,
    SourceID   INTEGER       NOT NULL,
    AimName    VARCHAR (20),
    AimDesc    VARCHAR (200),
    TableName  VARCHAR (20),
    t1         INTEGER,
    t2         INTEGER,
    t3         INTEGER,
    tmp        INTEGER,
    conditions VARCHAR,
    Status     INTEGER       DEFAULT (1) 
                             NOT NULL,
    Seq        INTEGER       NOT NULL
                             DEFAULT (999999),
    UNIQUE (
        RowID
    )
);

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           1,
                           36,
                           '村干部吃农村低保',
                           '村干部不是农村低保对象。',
                           '村干部吃农村低保',
                           36,
                           16,
                           0,
                           1,
                           '0',
                           1,
                           31050
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           2,
                           36,
                           '领导干部吃农村低保',
                           '领导干部不是农村低保对象。',
                           '领导干部吃农村低保',
                           36,
                           14,
                           0,
                           1,
                           '0',
                           1,
                           31030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           3,
                           36,
                           '财政供养人员吃农村低保',
                           '财政供养人员不是农村低保对象。',
                           '财政供养人员吃农村低保',
                           36,
                           12,
                           0,
                           1,
                           '0',
                           1,
                           31010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           4,
                           37,
                           '财政供养人员吃城市低保',
                           '财政供养人员不是城市低保对象。',
                           '财政供养人员吃城市低保',
                           37,
                           12,
                           0,
                           1,
                           '0',
                           1,
                           71010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           5,
                           37,
                           '领导干部吃城市低保',
                           '领导干部不是城市低保对象。',
                           '领导干部吃城市低保',
                           37,
                           14,
                           0,
                           1,
                           '0',
                           1,
                           71030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           6,
                           37,
                           '村干部吃城市低保',
                           '村干部不是城市低保对象。',
                           '村干部吃城市低保',
                           37,
                           16,
                           0,
                           1,
                           '0',
                           1,
                           71050
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           7,
                           5,
                           '财政供养人员吃农村五保',
                           '财政供养人员不是农村五保对象。',
                           '财政供养人员吃农村五保',
                           5,
                           12,
                           0,
                           1,
                           '0',
                           1,
                           201010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           8,
                           5,
                           '领导干部吃农村五保',
                           '领导干部不是农村五保对象',
                           '领导干部吃农村五保',
                           5,
                           14,
                           0,
                           1,
                           '0',
                           1,
                           201030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           9,
                           5,
                           '村干部吃农村五保',
                           '村干部不是农村五保对象',
                           '村干部吃农村五保',
                           5,
                           16,
                           0,
                           1,
                           '0',
                           1,
                           201050
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           40,
                           36,
                           '有工作吃农村低保',
                           '核实有工作吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '有工作吃农村低保',
                           36,
                           18,
                           0,
                           2,
                           '',
                           1,
                           31070
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           41,
                           36,
                           '有车吃农村低保',
                           '核实有车吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '有车吃农村低保',
                           36,
                           23,
                           0,
                           2,
                           '',
                           1,
                           31120
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           42,
                           36,
                           '购买农机吃农村低保',
                           '核实购买农机吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '购买农机吃农村低保',
                           36,
                           25,
                           0,
                           2,
                           '',
                           1,
                           31140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           43,
                           36,
                           '核查直系亲属有工作吃农村低保',
                           '核实直系亲属有工作吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有工作吃农村低保',
                           36,
                           18,
                           22,
                           3,
                           '',
                           1,
                           31070
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           44,
                           36,
                           '核查直系亲属有车吃农村低保',
                           '核实直系亲属有车吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有车吃农村低保',
                           36,
                           23,
                           22,
                           3,
                           '',
                           1,
                           31120
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           45,
                           37,
                           '有工作吃城市低保',
                           '核实有工作吃城市低保人员家庭经济情况，确认是否符合低保政策',
                           '有工作吃城市低保',
                           37,
                           18,
                           0,
                           2,
                           NULL,
                           1,
                           71070
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           46,
                           37,
                           '有公司吃城市低保',
                           '核实有公司吃城市低保人员家庭经济情况，确认是否符合低保政策',
                           '有公司吃城市低保',
                           37,
                           19,
                           0,
                           2,
                           NULL,
                           1,
                           71080
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           47,
                           37,
                           '有车吃城市低保',
                           '核实有车吃城市低保人员家庭经济情况，确认是否符合低保政策',
                           '有车吃城市低保',
                           37,
                           23,
                           0,
                           2,
                           NULL,
                           1,
                           71120
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           48,
                           37,
                           '购买农机吃城市低保',
                           '核实购买农机低保人员家庭经济情况，确认是否符合低保政策',
                           '购买农机吃城市低保',
                           37,
                           25,
                           0,
                           2,
                           NULL,
                           1,
                           71140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           50,
                           36,
                           '有房吃农村低保',
                           '核实有房吃农村低保人员家庭经济情况，确认是否符合低保政策',
                           '有房吃农村低保',
                           36,
                           27,
                           0,
                           2,
                           '',
                           1,
                           910140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           51,
                           5,
                           '有工作吃农村五保',
                           '核实有工作吃农村五保人员家庭经济情况，确认是否符合农村五保政策',
                           '有工作吃农村五保',
                           5,
                           18,
                           0,
                           2,
                           NULL,
                           1,
                           201070
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           52,
                           5,
                           '购买农机吃农村五保',
                           '核实购买农机吃农村五保人员家庭经济情况，确认是否符合农村五保政策',
                           '购买农机吃农村五保',
                           5,
                           25,
                           0,
                           2,
                           NULL,
                           1,
                           201140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           53,
                           5,
                           '有房吃农村五保',
                           '核实有房吃农村五保人员家庭经济情况，确认是否符合农村五保政策',
                           '有房吃农村五保',
                           5,
                           27,
                           0,
                           2,
                           NULL,
                           1,
                           201160
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           54,
                           5,
                           '有车吃农村五保',
                           '核实有车吃农村五保人员家庭经济情况，确认是否符合农村五保政策',
                           '有车吃农村五保',
                           5,
                           23,
                           0,
                           2,
                           NULL,
                           1,
                           201120
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           55,
                           5,
                           '有公司吃农村五保',
                           '核实有公司吃农村五保人员家庭经济情况，确认是否符合农村五保政策',
                           '有公司吃农村五保',
                           5,
                           19,
                           0,
                           2,
                           NULL,
                           1,
                           201080
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           74,
                           36,
                           '有公司吃农村低保',
                           '核实有公司吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '有公司吃农村低保',
                           36,
                           19,
                           0,
                           2,
                           '',
                           1,
                           31080
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           75,
                           36,
                           '核查直系亲属有公司吃农村低保',
                           '核实直系亲属有公司吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有公司吃农村低保',
                           36,
                           19,
                           22,
                           3,
                           '',
                           1,
                           31080
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           76,
                           36,
                           '核查直系亲属购买农机吃农村低保',
                           '核实直系亲属购买农机吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属购买农机吃农村低保',
                           36,
                           25,
                           22,
                           3,
                           '',
                           1,
                           31140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           77,
                           36,
                           '核查直系亲属有房吃农村低保',
                           '核实直系亲属有房吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有房吃农村低保',
                           36,
                           27,
                           22,
                           3,
                           NULL,
                           1,
                           31160
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           80,
                           36,
                           '死亡还吃农村低保',
                           '核查死亡人员超期吃农村低保情况',
                           '死亡超过90天还吃农村低保',
                           36,
                           24,
                           0,
                           5,
                           '60',
                           1,
                           31130
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           81,
                           36,
                           '农村低保身份证姓名不一致',
                           '比较公安人口信息，查找身份证一致而姓名不一致的低保人员。确认领低保人员的身份是否正确',
                           '农村低保身份证姓名不一致',
                           36,
                           44,
                           0,
                           4,
                           '',
                           1,
                           30020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           82,
                           36,
                           '农村低保身份证未查到',
                           '比较公安人口信息，未查到农村低保人身份证信息。确认领低保人员的身份是否正确',
                           '农村低保身份证未查到',
                           36,
                           43,
                           0,
                           7,
                           '',
                           1,
                           30010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           83,
                           36,
                           '领导干部家属吃农村低保',
                           '核查领导干部家属吃农村低保情况，确认其家庭收入是否符合政策规定',
                           '领导干部家属吃农村低保',
                           36,
                           15,
                           14,
                           8,
                           '0',
                           1,
                           31040
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           84,
                           36,
                           '村干部家属吃农村低保',
                           '核查村干部家属吃农村低保情况，确认其家庭收入是否符合政策规定',
                           '村干部家属吃农村低保',
                           36,
                           17,
                           16,
                           8,
                           '0',
                           1,
                           31060
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           86,
                           36,
                           '领退耕还林补贴金额大又吃农村低保',
                           '核实领退耕还林补贴金额大的家庭收入情况，确认是否符合农村低保条件',
                           '领退耕还林补贴又吃农村低保',
                           36,
                           40,
                           0,
                           9,
                           '3000',
                           1,
                           30090
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           88,
                           36,
                           '领生态公益林补贴金额大又吃农村低保',
                           '核实领生态公益林补贴金额大的家庭收入情况，确认是否符合农村低保条件',
                           '领生态公益林补贴又吃农村低保',
                           36,
                           35,
                           0,
                           9,
                           '3000',
                           1,
                           30100
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           89,
                           36,
                           '既吃城市低保又吃农村低保',
                           '城市低保和农村低保只能享受一种',
                           '吃城市低保又吃农村低保',
                           36,
                           37,
                           0,
                           10,
                           '0',
                           1,
                           30007
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           90,
                           37,
                           '城市低保身份证未查到',
                           '比较公安人口信息，未查到城市低保人身份证信息。确认领低保人员的身份是否正确',
                           '城市低保身份证未查到',
                           37,
                           43,
                           0,
                           7,
                           '',
                           1,
                           70010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           91,
                           37,
                           '城市低保身份证姓名不一致',
                           '比较公安人口信息，查找身份证一致而姓名不一致的城市低保人。确认领低保人员的身份是否正确',
                           '城市低保身份证姓名不一致',
                           37,
                           44,
                           0,
                           4,
                           '',
                           1,
                           70020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           92,
                           5,
                           '农村五保身份证未查到',
                           '比较公安人口信息，未查到农村五保人身份证信息。确认领农村五保人的身份是否正确',
                           '农村五保身份证未查到',
                           5,
                           43,
                           0,
                           7,
                           '',
                           1,
                           200010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           93,
                           5,
                           '农村五保身份证姓名不一致',
                           '比较公安人口信息，查找身份证一致而姓名不一致的农村五保人。确认领农村五保人的身份是否正确',
                           '农村五保身份证姓名不一致',
                           5,
                           44,
                           0,
                           4,
                           '',
                           1,
                           200020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           106,
                           37,
                           '领农业补贴金额大又吃城市低保',
                           '对领农业补贴大金额大的家庭收入情况，确认是否符合城市低保条件',
                           '领农业补贴又吃城市低保',
                           37,
                           10,
                           0,
                           9,
                           '1800',
                           1,
                           1010170
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           107,
                           37,
                           '领退耕还林补贴金额大又吃城市低保',
                           '核实领退耕还林补贴金额大的家庭收入情况，确认是否符合城市低保条件',
                           '领退耕还林补贴又吃城市低保',
                           37,
                           40,
                           0,
                           9,
                           '3000',
                           1,
                           70090
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           108,
                           37,
                           '领生态公益林补贴金额大又吃城市低保',
                           '核实领生态公益林补贴金额大的家庭收入情况，确认是否符合城市低保条件',
                           '领生态公益林补贴又吃城市低保',
                           37,
                           35,
                           0,
                           9,
                           '3000',
                           1,
                           70100
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           109,
                           37,
                           '领导干部家属吃城市低保',
                           '核查领导干部家属吃城市低保情况，确认其家庭收入是否符合政策规定',
                           '领导干部家属吃城市低保',
                           37,
                           15,
                           14,
                           8,
                           '0',
                           1,
                           71040
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           110,
                           37,
                           '村干部家属吃城市低保',
                           '核查村干部家属吃城市低保情况，确认其家庭收入是否符合政策规定',
                           '村干部家属吃城市低保',
                           37,
                           17,
                           16,
                           8,
                           '0',
                           1,
                           71060
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           111,
                           37,
                           '死亡还吃城市低保',
                           '核查死亡人员超期吃城市低保情况',
                           '死亡超过90天还吃城市低保',
                           37,
                           24,
                           0,
                           5,
                           '60',
                           1,
                           71130
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           112,
                           5,
                           '死亡还吃农村五保',
                           '核查死亡人员超期吃农村五保情况',
                           '死亡超过90天还吃农村五保',
                           5,
                           24,
                           0,
                           5,
                           '60',
                           1,
                           201130
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           113,
                           37,
                           '核查直系亲属有工作吃城市低保',
                           '核实直系亲属有工作吃城市低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有工作吃城市低保',
                           37,
                           18,
                           22,
                           3,
                           '',
                           1,
                           71070
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           114,
                           37,
                           '核查直系亲属有公司吃城市低保',
                           '核实直系亲属有公司吃城市低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有公司吃城市低保',
                           37,
                           19,
                           22,
                           3,
                           '',
                           1,
                           71080
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           115,
                           37,
                           '核查直系亲属有车吃城市低保',
                           '核实直系亲属有车吃城市低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有车吃城市低保',
                           37,
                           23,
                           22,
                           3,
                           '',
                           1,
                           71120
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           116,
                           37,
                           '核查直系亲属购买农机吃城市低保',
                           '核实直系亲属购买农机吃城市低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属购买农机吃城市低保',
                           37,
                           25,
                           22,
                           3,
                           '',
                           1,
                           71140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           117,
                           37,
                           '核查直系亲属有房吃城市低保',
                           '核实直系亲属有房吃城市低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有房吃城市低保',
                           37,
                           27,
                           22,
                           3,
                           '',
                           0,
                           71160
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           119,
                           5,
                           '既吃农村低保又吃农村五保',
                           '农村五保和农村低保只能享受一种',
                           '吃农村低保又吃农村五保',
                           5,
                           36,
                           0,
                           10,
                           '0',
                           1,
                           200003
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           120,
                           5,
                           '既吃城市低保又吃农村五保',
                           '农村五保和城市低保只能享受一种',
                           '吃城市低保又吃农村五保',
                           5,
                           37,
                           0,
                           10,
                           '0',
                           1,
                           200007
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           121,
                           36,
                           '领农业补贴金额大又吃农村低保',
                           '核实领农业补贴金额大的家庭收入情况，确认是否符合农村低保条件',
                           '领农业补贴又吃农村低保',
                           36,
                           10,
                           0,
                           9,
                           '1800',
                           1,
                           910170
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           122,
                           5,
                           '领农业补贴金额大又吃农村五保',
                           '对领农业补贴金额大的家庭收入情况，确认是否符合农村五保条件',
                           '领农业补贴又吃农村五保',
                           5,
                           10,
                           0,
                           9,
                           '200',
                           1,
                           200070
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           123,
                           5,
                           '领退耕还林补贴金额大又吃农村五保',
                           '对领退耕还林补贴金额大的家庭收入情况，确认是否符合农村五保条件',
                           '领退耕还林补贴又吃农村五保',
                           5,
                           40,
                           0,
                           9,
                           '3000',
                           1,
                           200090
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           124,
                           5,
                           '领生态公益林补贴金额大又吃农村五保',
                           '对领生态公益林补贴金额大的家庭收入情况，确认是否符合农村五保条件',
                           '领生态公益林补贴又吃农村五保',
                           5,
                           35,
                           0,
                           9,
                           '3000',
                           1,
                           200100
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           125,
                           5,
                           '领导干部家属吃农村五保',
                           '核查领导干部家属吃农村五保情况，确认其家庭收入是否符合政策规定',
                           '领导干部家属吃农村五保',
                           5,
                           15,
                           14,
                           8,
                           '0',
                           1,
                           201040
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           126,
                           5,
                           '村干部家属吃农村五保',
                           '核查村干部家属吃农村五保情况，确认其家庭收入是否符合政策规定',
                           '村干部家属吃农村五保',
                           5,
                           17,
                           16,
                           8,
                           '0',
                           1,
                           201060
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           127,
                           5,
                           '核查直系亲属有工作吃农村五保',
                           '核实直系亲属有工作吃农村五保人家庭经济情况，确认是否符合五保政策',
                           '直系亲属有工作吃农村五保',
                           5,
                           18,
                           22,
                           3,
                           '',
                           1,
                           201070
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           128,
                           5,
                           '核查直系亲属有公司吃农村五保',
                           '核实直系亲属有公司吃农村五保人家庭经济情况，确认是否符合五保政策',
                           '直系亲属有公司吃农村五保',
                           5,
                           19,
                           22,
                           3,
                           '',
                           1,
                           201080
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           129,
                           5,
                           '核查直系亲属有车吃农村五保',
                           '核实直系亲属有车吃农村五保人家庭经济情况，确认是否符合五保政策',
                           '直系亲属有车吃农村五保',
                           5,
                           23,
                           22,
                           3,
                           '',
                           1,
                           201120
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           130,
                           5,
                           '核查直系亲属购买农机吃农村五保',
                           '核实直系亲属购买农机吃农村五保人家庭经济情况，确认是否符合五保政策',
                           '直系亲属购买农机吃农村五保',
                           5,
                           25,
                           22,
                           3,
                           '',
                           1,
                           201140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           131,
                           5,
                           '核查直系亲属有房吃农村五保',
                           '核实直系亲属有房吃农村五保人家庭经济情况，确认是否符合五保政策',
                           '直系亲属有房吃农村五保',
                           5,
                           27,
                           22,
                           3,
                           '',
                           1,
                           201160
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           177,
                           5,
                           '既吃农村五保分散供养又吃农村集中供养',
                           '同时只能享受一种供养方式',
                           '吃农村五保又吃农村五保',
                           5,
                           5,
                           0,
                           12,
                           '0',
                           1,
                           200020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           179,
                           50,
                           '贫困户身份证未查到',
                           '比较公安人口信息，未查到贫困户人身份证信息。确认贫困户人员的身份是否正确',
                           '贫困户身份证未查到',
                           50,
                           43,
                           0,
                           7,
                           '',
                           1,
                           10
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           180,
                           50,
                           '贫困户身份证姓名不一致',
                           '比较公安人口信息，查找身份证一致而姓名不一致的贫困户。确认贫困户的身份是否正确',
                           '贫困户身份证姓名不一致',
                           50,
                           44,
                           0,
                           4,
                           '',
                           1,
                           20
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           181,
                           50,
                           '领农业补贴金额大的贫困户',
                           '核实领农业补贴金额大的家庭收入情况，确认是否符合贫困户条件',
                           '领农业补贴农业补贴又贫困户贫困户',
                           50,
                           10,
                           0,
                           16,
                           '1800',
                           1,
                           10170
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           182,
                           50,
                           '领退耕还林补贴金额大的贫困户',
                           '核实领退耕还林补贴金额大的家庭收入情况，确认是否符合贫困户条件',
                           '领退耕还林又贫困户贫困户',
                           50,
                           40,
                           0,
                           16,
                           '500',
                           1,
                           10180
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           183,
                           50,
                           '领生态公益林补贴金额大的贫困户',
                           '核实领生态公益林补贴金额大的家庭收入情况，确认是否符合贫困户条件',
                           '领生态公益林又贫困户贫困户',
                           50,
                           35,
                           0,
                           16,
                           '2000',
                           1,
                           10190
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           184,
                           50,
                           '财政供养人员是贫困户',
                           '核查财政供养人员家庭收入情况，确认是否符合贫困户标准。',
                           '财政供养人员贫困户贫困户',
                           50,
                           12,
                           0,
                           13,
                           '0',
                           1,
                           10010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           185,
                           50,
                           '领导干部是贫困户',
                           '核查领导干部家庭经济情况，确认是否符合贫困户标准。',
                           '领导干部贫困户贫困户',
                           50,
                           14,
                           0,
                           13,
                           '0',
                           1,
                           10020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           186,
                           50,
                           '领导干部家属是贫困户',
                           '核查领导干部家属经济情况，确认是否符合贫困胡标准',
                           '领导家属贫困户贫困户',
                           50,
                           15,
                           14,
                           14,
                           '0',
                           1,
                           10030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           187,
                           50,
                           '村干部是贫困户',
                           '核查村干部收入情况，确认是否符合贫困户标准。',
                           '村干部贫困户贫困户',
                           50,
                           16,
                           0,
                           13,
                           '0',
                           1,
                           10040
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           188,
                           50,
                           '村干部家属是贫困户',
                           '核查村干部家属的经济情况，确认是否符合贫困户标准',
                           '村家属贫困户贫困户',
                           50,
                           17,
                           16,
                           14,
                           '0',
                           1,
                           10050
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           189,
                           50,
                           '贫困户有工作收入',
                           '核实贫困户工作收入情况，确认是否符合贫困户标准',
                           '有工作贫困户贫困户',
                           50,
                           18,
                           0,
                           2,
                           '',
                           1,
                           10060
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           191,
                           50,
                           '贫困户直系亲属有工作收入',
                           '核查贫困户直系亲属工作收入情况，确认是否符合贫困户标准',
                           '直系亲属有工作贫困户贫困户',
                           50,
                           18,
                           22,
                           3,
                           '',
                           1,
                           10060
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           192,
                           50,
                           '贫困户有公司',
                           '核实贫困户有公司情况，确认是否符合低保政策',
                           '有公司贫困户贫困户',
                           50,
                           19,
                           0,
                           2,
                           '',
                           1,
                           10070
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           195,
                           50,
                           '贫困户直系亲属有公司',
                           '核实直系亲属有公司贫困户经济情况，确认是否符合贫困户标准',
                           '直系亲属有公司贫困户贫困户',
                           50,
                           19,
                           22,
                           3,
                           '',
                           1,
                           10070
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           196,
                           50,
                           '贫困户有车',
                           '核实贫困户家庭经济情况，确认是否符合贫困户标准',
                           '有车贫困户贫困户',
                           50,
                           23,
                           0,
                           2,
                           '',
                           1,
                           10100
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           197,
                           50,
                           '贫困户直系亲属有车',
                           '核实贫困户家庭经济情况，确认是否符合贫困户标准',
                           '直系亲属有车贫困户贫困户',
                           50,
                           23,
                           22,
                           3,
                           '',
                           1,
                           10100
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           198,
                           50,
                           '贫困户死亡情况',
                           '核查贫困户死亡情况',
                           '死亡超过90天还贫困户贫困户',
                           50,
                           24,
                           0,
                           15,
                           '60',
                           1,
                           10110
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           199,
                           50,
                           '贫困户购买农机情况',
                           '核实购买农机贫困户家庭经济情况，确认是否符合贫困户标准',
                           '购买农机贫困户贫困户',
                           50,
                           25,
                           0,
                           2,
                           '',
                           1,
                           10120
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           200,
                           50,
                           '贫困户直系亲属购买农机',
                           '核实直系亲属购买农机情况，确认是否符合贫困户标准',
                           '直系亲属购买农机贫困户贫困户',
                           50,
                           25,
                           22,
                           3,
                           '',
                           1,
                           10120
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           202,
                           50,
                           '贫困户购买商品房',
                           '核实购买商品房的贫困户家庭经济情况，确认是否符合贫困户标准',
                           '有房贫困户贫困户',
                           50,
                           27,
                           0,
                           2,
                           '',
                           1,
                           10140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           203,
                           50,
                           '贫困户直系亲属有房',
                           '核实直系亲属有房贫困户家庭经济情况，确认是否符合贫困户标准',
                           '直系亲属有房贫困户贫困户',
                           50,
                           27,
                           22,
                           3,
                           '',
                           1,
                           10140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           204,
                           52,
                           '非贫困户享受易地搬迁政策',
                           '核查非贫困户享受易地搬迁政策',
                           '贫困户易地扶贫搬迁易地扶贫搬迁',
                           52,
                           50,
                           0,
                           17,
                           '',
                           1,
                           200000
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           205,
                           52,
                           '重复享受易地扶贫搬迁',
                           '重复享受易地扶贫搬迁',
                           '财政供养人员易地扶贫搬迁易地扶贫搬迁',
                           52,
                           12,
                           0,
                           18,
                           '1',
                           1,
                           210010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           206,
                           52,
                           '享受异地扶贫搬迁又领危房改造资金',
                           '核查既享受异地扶贫搬迁又领危房改造资金',
                           '领易地扶贫搬迁易地扶贫搬迁',
                           52,
                           8,
                           0,
                           19,
                           '0',
                           1,
                           210220
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           207,
                           52,
                           '易地移民搬迁人均面积标准异常',
                           '核查不符标准的易地移民搬迁',
                           '面积大易地扶贫搬迁易地扶贫搬迁',
                           52,
                           45,
                           0,
                           20,
                           ' AND ((平均面积 > 25 * 1.1 AND 家庭人口 > 1) OR (平均面积 > 40 AND 家庭人口 = 1) OR (面积 > 125 AND    家庭人口 > 5)) ',
                           1,
                           200030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           208,
                           53,
                           '寄宿生领补助身份证未查到',
                           '核查寄宿生领补助是否真实',
                           '寄宿生生活费补助身份证未查到',
                           53,
                           43,
                           0,
                           7,
                           '',
                           1,
                           310010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           209,
                           53,
                           '寄宿生领补助姓名身份证不一致',
                           '核查寄宿生领补助是否真实',
                           '寄宿生生活费补助身份证姓名不一致',
                           53,
                           44,
                           0,
                           4,
                           '',
                           1,
                           310020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           210,
                           53,
                           '领寄宿补助未纳入建档立卡贫困户',
                           '核查领寄宿补助学生是否是贫困户',
                           '贫困户领寄宿生生活费补助',
                           53,
                           50,
                           0,
                           17,
                           '',
                           1,
                           310000
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           211,
                           53,
                           '领寄宿生补助年龄异常',
                           '核查年龄异常领寄宿生补助',
                           '无低保五保领寄宿生生活费补助',
                           53,
                           65,
                           0,
                           21,
                           '(年龄 >16 or 年龄 < 5)',
                           1,
                           310040
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           212,
                           54,
                           '领普通高中补助年龄异常',
                           '核查普通高中领补助人年龄异常情况。',
                           '无低保五保领高中助学金',
                           54,
                           65,
                           0,
                           21,
                           '(年龄 >19 or 年龄 < 15)',
                           1,
                           320040
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           213,
                           54,
                           '领普通高中助学金未纳入建档立卡贫困户',
                           '核查领普通高中助学金学生是否是贫困户',
                           '贫困户领高中助学金',
                           54,
                           50,
                           0,
                           17,
                           '',
                           1,
                           320000
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           214,
                           54,
                           '领普通高中助学金身份证未查到',
                           '核查领普通高中助学金是否真实',
                           '高中助学金身份证未查到',
                           54,
                           43,
                           0,
                           7,
                           '',
                           1,
                           320010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           215,
                           54,
                           '领普通高中助学金姓名身份证不一致',
                           '核查领普通高中助学金是否真实',
                           '高中助学金身份证姓名不一致',
                           54,
                           44,
                           0,
                           4,
                           '',
                           1,
                           320020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           216,
                           56,
                           '享受雨露计划学生未纳入建档立卡贫困户',
                           '享受雨露计划学生是否是贫困户',
                           '贫困户领雨露计划',
                           56,
                           50,
                           0,
                           17,
                           '',
                           1,
                           340000
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           217,
                           56,
                           '享受雨露计划学生身份证未查到',
                           '核查享受雨露计划学生是否真实',
                           '雨露计划身份证未查到',
                           56,
                           43,
                           0,
                           7,
                           '',
                           1,
                           340010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           218,
                           56,
                           '享受雨露计划学生姓名身份证不一致',
                           '核查雨露计划学生是否真实',
                           '雨露计划身份证姓名不一致',
                           56,
                           44,
                           0,
                           4,
                           '',
                           1,
                           340020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           219,
                           55,
                           '领中职助学金身份证未查到',
                           '核查领中职助学金是否真实',
                           '中职助学金身份证未查到',
                           55,
                           43,
                           0,
                           7,
                           '',
                           1,
                           330010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           220,
                           55,
                           '领中职助学金姓名身份证不一致',
                           '核查领中职助学金是否真实',
                           '中职助学金身份证姓名不一致',
                           55,
                           44,
                           0,
                           4,
                           '',
                           1,
                           330020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           221,
                           55,
                           '领中职助学金学生年龄异常',
                           '核查中职助学金学生年龄异常情况。',
                           '面积大领中职助学金',
                           55,
                           45,
                           0,
                           21,
                           '(年龄 >19 or 年龄 < 14)',
                           1,
                           330030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           222,
                           57,
                           '享受扶贫贷款人身份证未查到',
                           '核查享受扶贫贷款人是否真实',
                           '扶贫信贷身份证未查到',
                           57,
                           43,
                           0,
                           7,
                           '',
                           1,
                           400010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           223,
                           57,
                           '享享受扶贫贷款人姓名身份证不一致',
                           '核查享受扶贫贷款人是否真实',
                           '扶贫信贷身份证姓名不一致',
                           57,
                           44,
                           0,
                           4,
                           '',
                           1,
                           400020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           224,
                           57,
                           '申请扶贫信贷未纳入建档立卡贫困户',
                           '申请扶贫信贷是否是贫困户',
                           '贫困户领扶贫信贷',
                           57,
                           50,
                           0,
                           17,
                           '',
                           1,
                           400000
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           225,
                           57,
                           '核查扶贫信贷使用情况',
                           '核查扶贫信贷使用情况，是否用于发展',
                           '财政供养人员领扶贫信贷',
                           57,
                           12,
                           0,
                           23,
                           '',
                           0,
                           410010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           226,
                           52,
                           '易地扶贫搬迁身份证未查到',
                           '核查易地扶贫搬迁是否真实',
                           '易地扶贫搬迁身份证未查到',
                           52,
                           43,
                           0,
                           7,
                           '',
                           1,
                           200010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           227,
                           52,
                           '易地扶贫搬迁姓名身份证不一致',
                           '核查易地扶贫搬迁是否真实',
                           '易地扶贫搬迁身份证姓名不一致',
                           52,
                           44,
                           0,
                           4,
                           '',
                           1,
                           200020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           228,
                           58,
                           '基本公共卫生服务对象姓名身份证不一致',
                           '核查基本公共卫生服务对象是否真实',
                           '基本公共卫生服务身份证姓名不一致',
                           58,
                           44,
                           0,
                           4,
                           '',
                           1,
                           500020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           229,
                           58,
                           '基本公共卫生服务对象身份证未查到',
                           '核查基本公共卫生服务对象是否真实',
                           '基本公共卫生服务身份证未查到',
                           58,
                           43,
                           0,
                           7,
                           '',
                           1,
                           500010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           230,
                           58,
                           '核查住院期间服务情况',
                           '核查住院期间服务情况',
                           '住院领基本公共卫生服务',
                           58,
                           48,
                           0,
                           24,
                           '',
                           1,
                           510160
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           231,
                           69,
                           '扶贫到户资金身份证未查到',
                           '比较公安人口信息，核查扶贫到户资金人身份证信息。',
                           '扶贫到户身份证未查到',
                           69,
                           43,
                           0,
                           7,
                           '',
                           0,
                           1200010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           232,
                           69,
                           '领扶贫到户资金身份证姓名不一致',
                           '比较公安人口信息，查找身份证一致而姓名不一致的扶贫到户资金。',
                           '扶贫到户身份证姓名不一致',
                           69,
                           44,
                           0,
                           4,
                           '',
                           0,
                           1200020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           233,
                           69,
                           '非贫困户领扶贫到户资金',
                           '核查非贫困户领扶贫到户资金',
                           '贫困户扶贫到户资金扶贫到户',
                           69,
                           50,
                           0,
                           17,
                           '',
                           0,
                           1200000
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           235,
                           50,
                           '贫困户未领到扶贫到户资金',
                           '核查贫困户未领到扶贫到户资金',
                           '扶贫到户资金扶贫到户又贫困户贫困户',
                           50,
                           69,
                           0,
                           25,
                           '',
                           0,
                           120
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           236,
                           50,
                           '贫困户未领户户通',
                           '核查贫困户未领户户通',
                           '享受户户通又贫困户贫困户',
                           50,
                           68,
                           0,
                           25,
                           '',
                           1,
                           130
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           237,
                           68,
                           '非贫困户领户户通',
                           '核查非贫困户领户户通',
                           '贫困户享受户户通',
                           68,
                           50,
                           0,
                           17,
                           '',
                           1,
                           1300000
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           238,
                           68,
                           '户户通身份证未查到',
                           '比较公安人口信息，核查户户通身份证信息。',
                           '户户通身份证未查到',
                           68,
                           43,
                           0,
                           7,
                           '',
                           1,
                           1300010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           239,
                           68,
                           '领户户通身份证姓名不一致',
                           '比较公安人口信息，查找身份证一致而姓名不一致的领户户通贫困户。',
                           '户户通身份证姓名不一致',
                           68,
                           44,
                           0,
                           4,
                           '',
                           1,
                           1300020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           241,
                           59,
                           '核查既参加新农合又参加医保(居民职工)参保情况',
                           '核查既参加新农合又参加医保(居民职工)参保情况',
                           '医保领新农合',
                           59,
                           47,
                           0,
                           19,
                           '0',
                           1,
                           610150
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           242,
                           59,
                           '核查新农合刷卡次数',
                           '核查新农合刷卡次数',
                           '面积大领新农合',
                           59,
                           45,
                           0,
                           18,
                           '40',
                           1,
                           600030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           243,
                           60,
                           '职业介绍补贴对象身份证未查到',
                           '核查职业介绍补贴对象是否真实',
                           '职业介绍身份证未查到',
                           60,
                           43,
                           0,
                           7,
                           '',
                           1,
                           710010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           244,
                           60,
                           '职业介绍补贴对象姓名身份证不一致',
                           '核查职业介绍补贴对象是否真实',
                           '职业介绍身份证姓名不一致',
                           60,
                           44,
                           0,
                           4,
                           '',
                           1,
                           710020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           245,
                           61,
                           '职业培训补贴对象身份证未查到',
                           '核查职业培训补贴对象是否真实',
                           '职业培训身份证未查到',
                           61,
                           43,
                           0,
                           7,
                           '',
                           1,
                           720010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           246,
                           61,
                           '职业培训补贴对象姓名身份证不一致',
                           '核查职业培训补贴对象是否真实',
                           '职业培训身份证姓名不一致',
                           61,
                           44,
                           0,
                           4,
                           '',
                           1,
                           720020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           247,
                           62,
                           '创业培训补贴对象身份证未查到',
                           '核查创业培训补贴对象是否真实',
                           '创业培训身份证未查到',
                           62,
                           43,
                           0,
                           7,
                           '',
                           1,
                           730010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           248,
                           62,
                           '创业培训补贴对象姓名身份证不一致',
                           '核查创业培训补贴对象是否真实',
                           '创业培训身份证姓名不一致',
                           62,
                           44,
                           0,
                           4,
                           '',
                           1,
                           730020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           249,
                           63,
                           '困难残疾人身份证未查到',
                           '核查困难残疾人是否真实',
                           '困难残疾补贴身份证未查到',
                           63,
                           43,
                           0,
                           7,
                           '',
                           1,
                           810010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           250,
                           63,
                           '困难残疾人姓名身份证不一致',
                           '核查困难残疾人是否真实',
                           '困难残疾补贴身份证姓名不一致',
                           63,
                           44,
                           0,
                           4,
                           '',
                           1,
                           810020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           251,
                           64,
                           '重度残疾人身份证未查到',
                           '核查重度残疾人是否真实',
                           '重度残疾人护理补贴身份证未查到',
                           64,
                           43,
                           0,
                           7,
                           '',
                           1,
                           820010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           252,
                           64,
                           '重度残疾人姓名身份证不一致',
                           '核查重度残疾人是否真实',
                           '重度残疾人护理补贴身份证姓名不一致',
                           64,
                           44,
                           0,
                           4,
                           '',
                           1,
                           820020
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           253,
                           62,
                           '核查领创业培训只能领一次',
                           '创业培训只能领一次',
                           '面积大领创业培训',
                           62,
                           45,
                           0,
                           18,
                           '1',
                           1,
                           730030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           254,
                           60,
                           '核查职业介绍情况（一年领一次）',
                           '一年一次',
                           '财政供养人员领职业介绍',
                           60,
                           12,
                           0,
                           26,
                           '2',
                           1,
                           720010
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           255,
                           50,
                           '贫困户领养老保险',
                           '核查贫困户领养老保险情况',
                           '领养老保险贫困户贫困户',
                           50,
                           67,
                           0,
                           13,
                           '',
                           0,
                           10230
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           256,
                           62,
                           '创业培训是未开办公司',
                           '核查创业人是否领工商执照。',
                           '总金额领创业培训',
                           62,
                           66,
                           0,
                           23,
                           '',
                           1,
                           730030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           257,
                           61,
                           '核查职业介绍补贴领取情况（一年领一次）',
                           '一年一次',
                           '总金额领职业培训',
                           61,
                           66,
                           0,
                           26,
                           '2',
                           1,
                           720030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           258,
                           63,
                           '困难残疾人未纳入建档立卡贫困户',
                           '核查困难残疾人未纳入贫困户情况',
                           '贫困户领困难残疾补贴',
                           63,
                           50,
                           0,
                           17,
                           '',
                           1,
                           810000
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           259,
                           51,
                           '核查中标次数多的公司前20名',
                           '核查中标次数多的公司',
                           '面积大整村推进整村推进',
                           51,
                           45,
                           0,
                           27,
                           '20',
                           1,
                           100030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           260,
                           51,
                           '核查中标次数多的公司前5名(按乡镇)',
                           '核查中标次数多的公司(按乡镇)',
                           '总金额镇村推进镇村推进',
                           51,
                           66,
                           0,
                           28,
                           '5',
                           0,
                           100030
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           261,
                           50,
                           '五小所人员是贫困户',
                           '核查五小所人员收入情况，确认是否符合贫困户标准。',
                           '领贫困户贫困户',
                           50,
                           70,
                           0,
                           13,
                           '0',
                           1,
                           10240
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           262,
                           50,
                           '五小所人员家属是贫困户',
                           '核查五小所家属经济情况，确认是否符合贫困胡标准',
                           '五小所人员家属领贫困户贫困户',
                           50,
                           71,
                           14,
                           14,
                           '0',
                           1,
                           10250
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           263,
                           36,
                           '五小所人员吃农村低保',
                           '核查五小所人员是否是农村低保对象。',
                           '五小所人员领吃农村低保',
                           36,
                           70,
                           0,
                           1,
                           '0',
                           1,
                           910240
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           264,
                           36,
                           '核查五小所人员家属吃农村低保',
                           '核查五小所人员家属吃农村低保情况，确认其家庭收入是否符合政策规定',
                           '五小所人员领五小所人员家属领吃农村低保',
                           36,
                           71,
                           70,
                           8,
                           '0',
                           1,
                           910250
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           265,
                           37,
                           '五小所人员吃城市低保',
                           '核查五小所人员是不是城市低保对象。',
                           '五小所人员领吃城市低保',
                           37,
                           70,
                           0,
                           1,
                           '0',
                           1,
                           1010240
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           266,
                           37,
                           '五小所人员家属吃城市低保',
                           '核查五小所人员家属吃城市低保情况，确认其家庭收入是否符合政策规定',
                           '五小所人员领五小所人员家属领吃城市低保',
                           37,
                           71,
                           70,
                           8,
                           '0',
                           1,
                           1010250
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           267,
                           5,
                           '五小所人员吃农村五保',
                           '核查五小所人员是不是农村五保对象',
                           '五小所人员领吃农村五保',
                           5,
                           70,
                           0,
                           1,
                           '0',
                           1,
                           1110240
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           268,
                           5,
                           '五小所人员家属吃农村五保',
                           '核查五小所人员家属吃农村五保情况，确认其家庭收入是否符合政策规定',
                           '五小所人员领五小所人员家属领吃农村五保',
                           5,
                           71,
                           70,
                           8,
                           '0',
                           1,
                           1110250
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           269,
                           50,
                           '贫困户有工商登记',
                           '核实贫困户有工商登记，确认是否符合低保政策',
                           '有工商登记贫困户贫困户',
                           50,
                           46,
                           0,
                           2,
                           '',
                           1,
                           10210
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           270,
                           50,
                           '贫困户直系亲属有工商登记',
                           '核实贫困户直系亲属有工商登记的情况，确认是否符合贫困户标准',
                           '直系亲属有工商登记贫困户贫困户',
                           50,
                           46,
                           22,
                           3,
                           '',
                           1,
                           10210
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           271,
                           36,
                           '有工商登记吃农村低保',
                           '核实有工商登记吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '有工商登记吃农村低保',
                           36,
                           46,
                           0,
                           2,
                           '',
                           1,
                           910210
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           272,
                           36,
                           '核查直系亲属有工商登记吃农村低保',
                           '核实直系亲属有工商登记吃农村低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有工商登记吃农村低保',
                           36,
                           46,
                           22,
                           3,
                           '',
                           1,
                           910210
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           273,
                           37,
                           '有工商登记吃城市低保',
                           '核实有工商登记吃城市低保人员家庭经济情况，确认是否符合低保政策',
                           '有工商登记吃城市低保',
                           37,
                           46,
                           0,
                           2,
                           '',
                           1,
                           1010210
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           274,
                           37,
                           '核查直系亲属有工商登记吃城市低保情况',
                           '核实直系亲属有工商登记吃城市低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有工商登记吃城市低保',
                           37,
                           46,
                           22,
                           3,
                           '',
                           1,
                           1010210
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           275,
                           5,
                           '有工商登记吃农村五保',
                           '核实有工商登记吃农村五保人员家庭经济情况，确认是否符合农村五保政策',
                           '有工商登记吃农村五保',
                           5,
                           46,
                           0,
                           2,
                           '',
                           1,
                           1110210
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           276,
                           5,
                           '核查直系亲属有工商登记吃农村五保',
                           '核实直系亲属有工商登记吃农村五保人家庭经济情况，确认是否符合五保政策',
                           '直系亲属有工商登记吃农村五保',
                           5,
                           46,
                           22,
                           3,
                           '',
                           1,
                           1110210
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           277,
                           37,
                           '有房吃城市低保',
                           '核实有房吃城市低保人员家庭经济情况，确认是否符合低保政策',
                           '有房吃城市低保',
                           37,
                           27,
                           0,
                           2,
                           '',
                           0,
                           1010140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           278,
                           37,
                           '核查家庭有2套房吃城市低保',
                           '核实家庭有2套房吃城市低保人家庭经济情况，确认是否符合低保政策',
                           '直系亲属有房吃城市低保',
                           37,
                           27,
                           22,
                           29,
                           '1',
                           1,
                           1010140
                       );

INSERT INTO CompareAim (
                           RowID,
                           SourceID,
                           AimName,
                           AimDesc,
                           TableName,
                           t1,
                           t2,
                           t3,
                           tmp,
                           conditions,
                           Status,
                           Seq
                       )
                       VALUES (
                           279,
                           61,
                           '核查职业培训时间异常情况',
                           '核查职业培训时间异常情况',
                           '面积大领职业培训',
                           61,
                           45,
                           0,
                           30,
                           '',
                           1,
                           720030
                       );


-- 表：DataCheckRules
DROP TABLE IF EXISTS DataCheckRules;

CREATE TABLE DataCheckRules (
    RowID     INTEGER       PRIMARY KEY AUTOINCREMENT,
    CheckName VARCHAR (15)  UNIQUE,
    CheckSql  VARCHAR (500) NOT NULL,
    Status    INTEGER       DEFAULT (1) 
                            NOT NULL,
    Type      INTEGER       DEFAULT (1),
    Seq       INTEGER       NOT NULL
                            DEFAULT (99999999999) 
);

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               1,
                               '删除财政供养人员中领导干部的信息',
                               'DELETE FROM tbCompareCivilInfo
      WHERE ID IN
(SELECT a.ID
   FROM tbCompareCivilInfo a
        INNER JOIN tbCompareLeaderInfo b ON a.ID = b.ID
  WHERE a.Name = b.Name)',
                               1,
                               10,
                               1
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               2,
                               '删除财政供养人员中村干部的信息',
                               'DELETE FROM tbCompareCivilInfo
      WHERE ID IN
(SELECT a.ID
   FROM tbCompareCivilInfo a
        INNER JOIN tbComparecountryInfo b ON a.ID = b.ID
  WHERE a.Name = b.Name)',
                               1,
                               10,
                               2
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               3,
                               '领导干部身份信息校验',
                               'SELECT DISTINCT '''' AS 序号,
                ''领导干部身份证未查到'' AS 类型,
                tbCompareLeaderInfo.Addr AS 单位,
                tbCompareLeaderInfo.ID AS 身份证号,
                tbCompareLeaderInfo.Name AS 姓名,
                '''' AS 公安姓名
  FROM tbCompareLeaderInfo
 WHERE tbCompareLeaderInfo.ID NOT IN
       (SELECT tbComparePersonInfo.ID
          FROM tbComparePersonInfo) 
UNION ALL
SELECT DISTINCT '''' AS 序号,
                ''领导干部身份证姓名不一致'' AS 类型,
                tbCompareLeaderInfo.Addr AS 单位,
                tbCompareLeaderInfo.ID AS 身份证号,
                tbCompareLeaderInfo.Name AS 姓名,
                tbComparePersonInfo.Name AS 公安姓名
  FROM tbCompareLeaderInfo
       JOIN tbComparePersonInfo ON tbCompareLeaderInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareLeaderInfo.Name AND 
       (length(tbCompareLeaderInfo.ID) = 15 OR 
        length(tbCompareLeaderInfo.ID) = 18) 
 ORDER BY 单位,
          身份证号',
                               1,
                               1,
                               3
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               4,
                               '领导干部家属身份信息校验',
                               'SELECT DISTINCT '''' AS 序号,''领导干部家属身份证未查到'' AS 类型,
                tbCompareLeaderRelateInfo.Addr AS 单位,
                tbCompareLeaderRelateInfo.ID AS 身份证号,
                tbCompareLeaderRelateInfo.Name AS 姓名,
                '''' AS 公安姓名
  FROM tbCompareLeaderRelateInfo
 WHERE tbCompareLeaderRelateInfo.ID NOT IN
       (SELECT tbComparePersonInfo.ID
          FROM tbComparePersonInfo) 
UNION ALL
SELECT DISTINCT '''' AS 序号,''领导干部家属身份证姓名不一致'' AS 类型,
                tbCompareLeaderRelateInfo.Addr AS 单位,
                tbCompareLeaderRelateInfo.ID AS 身份证号,
                tbCompareLeaderRelateInfo.Name AS 姓名,
                tbComparePersonInfo.Name AS 公安姓名
  FROM tbCompareLeaderRelateInfo
       JOIN tbComparePersonInfo ON tbCompareLeaderRelateInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareLeaderRelateInfo.Name AND 
       (length(tbCompareLeaderRelateInfo.ID) = 15 OR 
        length(tbCompareLeaderRelateInfo.ID) = 18) 
 ORDER BY 单位,
          身份证号',
                               1,
                               1,
                               4
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               5,
                               '村干部身份信息校验',
                               'SELECT DISTINCT '''' AS 序号,
                ''村干部身份证未查到'' AS 类型,
                tbComparecountryInfo.Region AS 乡镇街道,
                tbComparecountryInfo.Addr AS 单位,
                tbComparecountryInfo.ID AS 身份证号,
                tbComparecountryInfo.Name AS 姓名,
                '''' AS 公安姓名
  FROM tbComparecountryInfo
 WHERE tbComparecountryInfo.ID NOT IN (
           SELECT tbComparePersonInfo.ID
             FROM tbComparePersonInfo
       )
UNION ALL
SELECT DISTINCT '''' AS 序号,
                ''村干部身份证姓名不一致'' AS 类型,
                tbComparecountryInfo.Region AS 乡镇街道,
                tbComparecountryInfo.Addr AS 单位,
                tbComparecountryInfo.ID AS 身份证号,
                tbComparecountryInfo.Name AS 姓名,
                tbComparePersonInfo.Name AS 公安姓名
  FROM tbComparecountryInfo
       JOIN
       tbComparePersonInfo ON tbComparecountryInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbComparecountryInfo.Name AND 
       (length(tbComparecountryInfo.ID) = 15 OR 
        length(tbComparecountryInfo.ID) = 18) 
 ORDER BY 单位,
          身份证号;
',
                               1,
                               1,
                               5
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               6,
                               '村干部家属身份信息校验',
                               'SELECT DISTINCT '''' AS 序号,
                ''村干部家属身份证未查到'' AS 类型,
                tbComparecountryRelateInfo.Region AS 乡镇街道,
                tbComparecountryRelateInfo.RelateName AS 村干部姓名,
                tbComparecountryRelateInfo.Addr AS 单位,
                tbComparecountryRelateInfo.ID AS 身份证号,
                tbComparecountryRelateInfo.Name AS 姓名,
                '''' AS 公安姓名
  FROM tbComparecountryRelateInfo
 WHERE tbComparecountryRelateInfo.ID NOT IN (
           SELECT tbComparePersonInfo.ID
             FROM tbComparePersonInfo
       )
UNION ALL
SELECT DISTINCT '''' AS 序号,
                ''村干部家属身份证姓名不一致'' AS 类型,
                tbComparecountryRelateInfo.Region AS 乡镇街道,
                tbComparecountryRelateInfo.RelateName AS 村干部姓名,
                tbComparecountryRelateInfo.Addr AS 单位,
                tbComparecountryRelateInfo.ID AS 身份证号,
                tbComparecountryRelateInfo.Name AS 姓名,
                tbComparePersonInfo.Name AS 公安姓名
  FROM tbComparecountryRelateInfo
       JOIN
       tbComparePersonInfo ON tbComparecountryRelateInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbComparecountryRelateInfo.Name AND 
       (length(tbComparecountryRelateInfo.ID) = 15 OR 
        length(tbComparecountryRelateInfo.ID) = 18) 
 ORDER BY 单位,
          身份证号;
',
                               1,
                               1,
                               6
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               7,
                               '无家属信息的五小所人员',
                               'SELECT DISTINCT '''' AS 序号,
                region AS 乡镇街道,
                Addr AS 单位,
                ID AS 身份证号,
                Name AS 姓名
  FROM tbCompareTownFive
 WHERE id NOT IN (
           SELECT b.RelateID
             FROM tbCompareTownFive a,
                  tbCompareTownFiveRelate b
            WHERE a.ID == b.RelateID
       )
 ORDER BY 乡镇街道',
                               1,
                               1,
                               7
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               8,
                               '财政供养人员身份信息校验',
                               'SELECT DISTINCT '''' AS 序号,
                ''财政供养人员身份证未查到'' AS 类型,
                tbCompareCivilInfo.Addr AS 单位,
                tbCompareCivilInfo.ID AS 身份证号,
                tbCompareCivilInfo.Name AS 姓名,
                '''' AS 公安姓名
  FROM tbCompareCivilInfo
 WHERE tbCompareCivilInfo.ID NOT IN
       (SELECT tbComparePersonInfo.ID
          FROM tbComparePersonInfo) 
UNION ALL
SELECT DISTINCT '''' AS 序号,
                ''财政供养人员身份证姓名不一致'' AS 类型,
                tbCompareCivilInfo.Addr AS 单位,
                tbCompareCivilInfo.ID AS 身份证号,
                tbCompareCivilInfo.Name AS 姓名,
                tbComparePersonInfo.Name AS 公安姓名
  FROM tbCompareCivilInfo
       JOIN tbComparePersonInfo ON tbCompareCivilInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareCivilInfo.Name AND 
       (length(tbCompareCivilInfo.ID) = 15 OR 
        length(tbCompareCivilInfo.ID) = 18) 
 ORDER BY 单位,
          身份证号',
                               1,
                               1,
                               8
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               10,
                               '无家属信息领导',
                               'SELECT DISTINCT '''' AS 序号,
                Addr AS 单位,
                ID AS 身份证号,
                Name AS 姓名
  FROM tbCompareLeaderInfo
 WHERE id NOT IN
       (SELECT a.ID
          FROM tbCompareLeaderInfo a,
               tbCompareLeaderRelateInfo b
         WHERE a.ID == b.RelateID)',
                               1,
                               1,
                               9
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               11,
                               '无家属信息村干部',
                               'SELECT DISTINCT '''' AS 序号,
                region 乡镇街道,
                Addr AS 单位,
                ID AS 身份证号,
                Name AS 姓名
  FROM tbComparecountryInfo
 WHERE id NOT IN (
           SELECT a.ID
             FROM tbComparecountryInfo a,
                  tbComparecountryRelateInfo b
            WHERE a.ID == b.RelateID
       )
 ORDER BY 乡镇街道',
                               1,
                               1,
                               10
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               12,
                               '五小所人员信息校验',
                               'SELECT DISTINCT '''' AS 序号,
                ''五小所人员身份证未查到'' AS 类型,
                tbCompareTownFive.Addr AS 单位,
                tbCompareTownFive.ID AS 身份证号,
                tbCompareTownFive.Name AS 姓名,
                '''' AS 公安姓名
  FROM tbCompareTownFive
 WHERE tbCompareTownFive.ID NOT IN (
           SELECT tbComparePersonInfo.ID
             FROM tbComparePersonInfo
       )
UNION ALL
SELECT DISTINCT '''' AS 序号,
                ''五小所人员身份证姓名不一致'' AS 类型,
                tbCompareTownFive.Addr AS 单位,
                tbCompareTownFive.ID AS 身份证号,
                tbCompareTownFive.Name AS 姓名,
                tbComparePersonInfo.Name AS 公安姓名
  FROM tbCompareTownFive
       JOIN
       tbComparePersonInfo ON tbCompareTownFive.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareTownFive.Name AND 
       (length(tbCompareTownFive.ID) = 15 OR 
        length(tbCompareTownFive.ID) = 18) 
 ORDER BY 单位,
          身份证号',
                               1,
                               1,
                               99999999999
                           );

INSERT INTO DataCheckRules (
                               RowID,
                               CheckName,
                               CheckSql,
                               Status,
                               Type,
                               Seq
                           )
                           VALUES (
                               13,
                               '五小所人员家属信息校验',
                               'SELECT DISTINCT '''' AS 序号,
                ''领导干部家属身份证未查到'' AS 类型,
                tbCompareTownFiveRelate.Addr AS 单位,
                tbCompareTownFiveRelate.ID AS 身份证号,
                tbCompareTownFiveRelate.Name AS 姓名,
                '''' AS 公安姓名
  FROM tbCompareTownFiveRelate
 WHERE tbCompareTownFiveRelate.ID NOT IN (
           SELECT tbComparePersonInfo.ID
             FROM tbComparePersonInfo
       )
UNION ALL
SELECT DISTINCT '''' AS 序号,
                ''领导干部家属身份证姓名不一致'' AS 类型,
                tbCompareTownFiveRelate.Addr AS 单位,
                tbCompareTownFiveRelate.ID AS 身份证号,
                tbCompareTownFiveRelate.Name AS 姓名,
                tbComparePersonInfo.Name AS 公安姓名
  FROM tbCompareTownFiveRelate
       JOIN
       tbComparePersonInfo ON tbCompareTownFiveRelate.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareTownFiveRelate.Name AND 
       (length(tbCompareTownFiveRelate.ID) = 15 OR 
        length(tbCompareTownFiveRelate.ID) = 18) 
 ORDER BY 单位,
          身份证号',
                               1,
                               1,
                               99999999999
                           );


-- 表：CompareAimTest
DROP TABLE IF EXISTS CompareAimTest;

CREATE TABLE CompareAimTest (
    AimID        INTEGER PRIMARY KEY,
    TestResult   INTEGER,
    TheoryResult INTEGER,
    TestTime     VARCHAR
);

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               1,
                               0,
                               1,
                               '15.9754'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               2,
                               0,
                               1,
                               '18.0068'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               3,
                               2,
                               2,
                               '15.6335'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               4,
                               0,
                               1,
                               '15.4152'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               5,
                               0,
                               1,
                               '17.2958'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               6,
                               0,
                               1,
                               '17.3853'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               7,
                               0,
                               1,
                               '15.9974'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               8,
                               0,
                               1,
                               '19.0014'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               9,
                               0,
                               1,
                               '14.0072'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               40,
                               0,
                               1,
                               '14.1183'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               41,
                               6,
                               1,
                               '16.296'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               42,
                               3,
                               1,
                               '16.372'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               43,
                               0,
                               1,
                               '23.8636'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               44,
                               4,
                               1,
                               '24.0027'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               45,
                               0,
                               1,
                               '17.0163'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               46,
                               37,
                               1,
                               '50.957'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               47,
                               6,
                               1,
                               '17.8593'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               48,
                               3,
                               1,
                               '17.0028'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               50,
                               23,
                               1,
                               '19'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               51,
                               0,
                               1,
                               '17.0173'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               52,
                               2,
                               1,
                               '15.9983'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               53,
                               3,
                               1,
                               '14.9743'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               54,
                               4,
                               1,
                               '16.0002'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               55,
                               20,
                               1,
                               '62.0237'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               72,
                               23,
                               1,
                               '18.3613'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               74,
                               37,
                               1,
                               '57.0001'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               75,
                               17,
                               1,
                               '43.018'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               76,
                               2,
                               1,
                               '24.082'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               77,
                               9,
                               1,
                               '30.9643'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               80,
                               2,
                               2,
                               '31.2507'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               81,
                               0,
                               1,
                               '26.0013'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               82,
                               0,
                               1,
                               '18.9758'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               83,
                               2,
                               2,
                               '15.6312'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               84,
                               1,
                               0,
                               '15.9979'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               86,
                               0,
                               1,
                               '31.2465'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               88,
                               0,
                               1,
                               '22.9973'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               89,
                               2,
                               2,
                               '15.6419'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               90,
                               0,
                               1,
                               '17.0522'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               91,
                               0,
                               1,
                               '24.0433'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               92,
                               0,
                               1,
                               '17.5473'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               93,
                               0,
                               1,
                               '20.2918'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               106,
                               2,
                               2,
                               '31.2488'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               107,
                               0,
                               1,
                               '22.966'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               108,
                               0,
                               1,
                               '23.8813'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               109,
                               0,
                               1,
                               '18.0152'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               110,
                               1,
                               0,
                               '17.0084'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               111,
                               0,
                               1,
                               '18.2233'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               112,
                               0,
                               1,
                               '18.0021'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               113,
                               0,
                               1,
                               '27.0128'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               114,
                               17,
                               1,
                               '43.0037'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               115,
                               4,
                               1,
                               '24.9758'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               116,
                               2,
                               1,
                               '28.9983'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               117,
                               9,
                               1,
                               '25.001'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               119,
                               0,
                               1,
                               '20.0324'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               120,
                               0,
                               1,
                               '22.9665'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               121,
                               0,
                               1,
                               '25.9976'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               122,
                               0,
                               1,
                               '20.9973'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               123,
                               0,
                               1,
                               '22.0035'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               124,
                               0,
                               1,
                               '26.9988'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               125,
                               0,
                               1,
                               '18.9982'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               126,
                               2,
                               1,
                               '17.9955'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               127,
                               0,
                               1,
                               '24.981'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               128,
                               11,
                               1,
                               '47.9768'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               129,
                               1,
                               0,
                               '24.9983'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               130,
                               1,
                               0,
                               '28.0247'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               131,
                               4,
                               1,
                               '28.0232'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               177,
                               2,
                               2,
                               '15.592'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               179,
                               2,
                               2,
                               '23.9989'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               180,
                               2,
                               2,
                               '22.9973'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               181,
                               2,
                               2,
                               '15.5547'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               182,
                               0,
                               0,
                               '15.6904'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               183,
                               0,
                               0,
                               '15.5981'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               184,
                               1,
                               1,
                               '0'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               185,
                               2,
                               2,
                               '15.5831'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               186,
                               2,
                               2,
                               '15.697'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               187,
                               1,
                               1,
                               '15.5635'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               188,
                               2,
                               2,
                               '15.5906'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               189,
                               2,
                               2,
                               '15.6452'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               191,
                               2,
                               2,
                               '15.64'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               192,
                               3,
                               3,
                               '46.8516'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               195,
                               0,
                               0,
                               '31.2731'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               196,
                               0,
                               0,
                               '15.598'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               197,
                               0,
                               0,
                               '31.2578'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               198,
                               2,
                               2,
                               '15.607'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               199,
                               0,
                               0,
                               '15.6353'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               200,
                               0,
                               0,
                               '31.2186'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               202,
                               0,
                               0,
                               '0'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               203,
                               0,
                               0,
                               '31.3258'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               206,
                               1,
                               1,
                               '31.2507'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               207,
                               7,
                               7,
                               '15.6139'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               211,
                               4,
                               4,
                               '31.2092'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               230,
                               0,
                               1,
                               '15.6256'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               235,
                               2,
                               2,
                               '15.5654'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               236,
                               2,
                               2,
                               '25.0179'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               237,
                               1,
                               1,
                               '17.9834'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               242,
                               0,
                               '',
                               '31.2274'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               254,
                               1,
                               1,
                               '15.6078'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               255,
                               1,
                               1,
                               '15.6611'
                           );

INSERT INTO CompareAimTest (
                               AimID,
                               TestResult,
                               TheoryResult,
                               TestTime
                           )
                           VALUES (
                               256,
                               13,
                               13,
                               '15.6364'
                           );


-- 表：DataFormat
DROP TABLE IF EXISTS DataFormat;

CREATE TABLE DataFormat (
    RowID       INTEGER      PRIMARY KEY,
    ParentID    INTEGER,
    ColCode     VARCHAR (20),
    ColName     VARCHAR (20),
    DisplayName VARCHAR (20),
    Col         INTEGER,
    Comment     VARCHAR (20),
    Seq         INTEGER      NOT NULL
                             DEFAULT (999999),
    UNIQUE (
        RowID
    )
);

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           35,
                           20,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           4,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           36,
                           20,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           37,
                           20,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           38,
                           20,
                           'Addr',
                           '地址',
                           '地址',
                           10,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           103,
                           9,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           4,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           104,
                           9,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           105,
                           9,
                           'Name',
                           '姓名',
                           '姓名',
                           1,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           106,
                           9,
                           'DataDate',
                           '日期',
                           '日期',
                           3,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           107,
                           9,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           108,
                           9,
                           'Type',
                           '型号',
                           '型号',
                           4,
                           NULL,
                           10
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           117,
                           11,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           5,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           118,
                           11,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           119,
                           11,
                           'DataDate',
                           '日期',
                           '日期',
                           1,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           120,
                           11,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           121,
                           11,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           122,
                           11,
                           'Area',
                           '面积',
                           '面积',
                           5,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           123,
                           11,
                           'Amount',
                           '金额',
                           '金额',
                           9,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           124,
                           11,
                           'LinkCol',
                           '关联列号',
                           '关联列号',
                           0,
                           '',
-                          1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           328,
                           23,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           329,
                           23,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           330,
                           23,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           331,
                           23,
                           'Type',
                           '型号',
                           '型号',
                           3,
                           NULL,
                           10
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           410,
                           12,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           411,
                           12,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           412,
                           12,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           413,
                           12,
                           'Addr',
                           '地址',
                           '单位',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           414,
                           14,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           415,
                           14,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           416,
                           14,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           417,
                           14,
                           'Addr',
                           '地址',
                           '单位',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           418,
                           14,
                           'Type',
                           '型号',
                           '现任职务',
                           4,
                           NULL,
                           11
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           441,
                           18,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           442,
                           18,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           443,
                           18,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           444,
                           18,
                           'Amount',
                           '金额',
                           '金额',
                           3,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           445,
                           18,
                           'DataDate',
                           '日期',
                           '日期',
                           4,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           469,
                           26,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           4,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           470,
                           26,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           471,
                           26,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           472,
                           26,
                           'DataDate',
                           '日期',
                           '日期',
                           3,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           481,
                           16,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           482,
                           16,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           483,
                           16,
                           'Name',
                           '姓名',
                           '姓名',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           484,
                           16,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           485,
                           16,
                           'Addr',
                           '地址',
                           '单位',
                           2,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           486,
                           16,
                           'Number',
                           '编号',
                           '职务',
                           5,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           487,
                           42,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           488,
                           42,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           489,
                           42,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           490,
                           42,
                           'Addr',
                           '地址',
                           '单位',
                           1,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           499,
                           36,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           500,
                           36,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           501,
                           36,
                           'Name',
                           '姓名',
                           '农村低保姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           502,
                           36,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           503,
                           36,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           504,
                           36,
                           'DataDate',
                           '日期',
                           '日期',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           505,
                           36,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           521,
                           6,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           522,
                           6,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           523,
                           6,
                           'Name',
                           '姓名',
                           '救助姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           524,
                           6,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           525,
                           6,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           526,
                           6,
                           'DataDate',
                           '日期',
                           '日期',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           527,
                           6,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           528,
                           5,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           529,
                           5,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           530,
                           5,
                           'Name',
                           '姓名',
                           '五保姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           531,
                           5,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           532,
                           5,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           533,
                           5,
                           'DataDate',
                           '日期',
                           '日期',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           534,
                           5,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           535,
                           5,
                           'AmountType',
                           '补助类型',
                           '补助类型',
                           7,
                           NULL,
                           7
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           536,
                           37,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           537,
                           37,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           538,
                           37,
                           'Name',
                           '姓名',
                           '城市低保姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           539,
                           37,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           540,
                           37,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           541,
                           37,
                           'DataDate',
                           '日期',
                           '日期',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           542,
                           37,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           543,
                           7,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           544,
                           7,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           545,
                           7,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           546,
                           7,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           547,
                           7,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           548,
                           7,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           549,
                           7,
                           'DataDate',
                           '日期',
                           '日期',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           550,
                           8,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           551,
                           8,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           552,
                           8,
                           'Name',
                           '姓名',
                           '补贴姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           553,
                           8,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           554,
                           8,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           555,
                           8,
                           'DataDate',
                           '日期',
                           '日期',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           556,
                           8,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           578,
                           24,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           579,
                           24,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           580,
                           24,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           581,
                           24,
                           'DataDate',
                           '日期',
                           '日期',
                           3,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           609,
                           47,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           610,
                           47,
                           'InputID',
                           '身份证号',
                           '参保人身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           611,
                           47,
                           'Name',
                           '姓名',
                           '参保人姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           612,
                           47,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           613,
                           47,
                           'Addr',
                           '地址',
                           '家庭地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           614,
                           47,
                           'DataDate',
                           '日期',
                           '日期',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           615,
                           47,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           616,
                           48,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           617,
                           48,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           618,
                           48,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           619,
                           48,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           620,
                           48,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           621,
                           48,
                           'DataDate',
                           '日期',
                           '入院时间',
                           5,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           622,
                           48,
                           'DataDate1',
                           '日期1',
                           '出院时间',
                           6,
                           NULL,
                           14
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           631,
                           40,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           7,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           632,
                           40,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           633,
                           40,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           634,
                           40,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           635,
                           40,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           636,
                           40,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           637,
                           40,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           638,
                           40,
                           'Area',
                           '面积',
                           '面积',
                           5,
                           NULL,
                           13
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           639,
                           35,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           640,
                           35,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           641,
                           35,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           642,
                           35,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           643,
                           35,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           644,
                           35,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           645,
                           35,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           646,
                           35,
                           'Area',
                           '面积',
                           '面积',
                           5,
                           NULL,
                           13
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           647,
                           49,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           648,
                           49,
                           'InputID',
                           '身份证号',
                           '法人身份证号',
                           4,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           649,
                           49,
                           'Name',
                           '姓名',
                           '法人姓名',
                           5,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           650,
                           49,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           651,
                           49,
                           'Type',
                           '型号',
                           '资质类型',
                           9,
                           NULL,
                           11
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           652,
                           49,
                           'Number',
                           '编号',
                           '资质编号',
                           8,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           653,
                           49,
                           'Serial1',
                           '编号1',
                           '组织机构代码',
                           1,
                           NULL,
                           15
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           654,
                           49,
                           'Serial2',
                           '编号2',
                           '营业制造',
                           2,
                           NULL,
                           16
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           655,
                           49,
                           'Relation',
                           '关系',
                           '统一社会信用编码',
                           3,
                           NULL,
                           10
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           667,
                           46,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           668,
                           46,
                           'InputID',
                           '身份证号',
                           '法人身份证号',
                           4,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           669,
                           46,
                           'Name',
                           '姓名',
                           '法人姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           670,
                           46,
                           'Addr',
                           '地址',
                           '营业地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           671,
                           46,
                           'RelateName',
                           '相关人姓名',
                           '企业名称',
                           2,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           672,
                           46,
                           'Serial2',
                           '编号2',
                           '注册号',
                           1,
                           NULL,
                           16
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           673,
                           50,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           674,
                           50,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           675,
                           50,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           676,
                           50,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           677,
                           50,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           678,
                           50,
                           'Relation',
                           '关系',
                           '关系',
                           5,
                           NULL,
                           10
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           679,
                           52,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           680,
                           52,
                           'InputID',
                           '身份证号',
                           '搬迁户身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           681,
                           52,
                           'Name',
                           '姓名',
                           '搬迁户姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           682,
                           52,
                           'Region',
                           '乡镇街道',
                           '搬迁户乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           683,
                           52,
                           'Addr',
                           '地址',
                           '搬迁户地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           684,
                           52,
                           'DataDate',
                           '日期',
                           '搬迁日期',
                           5,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           685,
                           52,
                           'Amount',
                           '金额',
                           '家庭人数',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           686,
                           52,
                           'AmountType',
                           '补助类型',
                           '安装方式',
                           7,
                           NULL,
                           7
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           687,
                           52,
                           'Area',
                           '面积',
                           '面积',
                           8,
                           NULL,
                           13
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           688,
                           53,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           689,
                           53,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           690,
                           53,
                           'Name',
                           '姓名',
                           '姓名',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           691,
                           53,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           692,
                           53,
                           'Addr',
                           '地址',
                           '地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           693,
                           53,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           694,
                           53,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           695,
                           53,
                           'Type',
                           '型号',
                           '型号',
                           2,
                           NULL,
                           11
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           704,
                           57,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           705,
                           57,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           706,
                           57,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           707,
                           57,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           708,
                           57,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           709,
                           57,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           710,
                           57,
                           'DataDate',
                           '日期',
                           '日期',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           724,
                           69,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           725,
                           69,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           726,
                           69,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           727,
                           69,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           728,
                           69,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           729,
                           69,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           783,
                           10,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           784,
                           10,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           785,
                           10,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           786,
                           10,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           787,
                           10,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           788,
                           10,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           789,
                           10,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           790,
                           10,
                           'Area',
                           '面积',
                           '面积',
                           5,
                           NULL,
                           13
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           791,
                           15,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           792,
                           15,
                           'InputID',
                           '身份证号',
                           '家属身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           793,
                           15,
                           'Name',
                           '姓名',
                           '家属姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           794,
                           15,
                           'Addr',
                           '地址',
                           '领导单位',
                           3,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           795,
                           15,
                           'RelateID',
                           '相关人身份证号',
                           '领导身份证号',
                           4,
                           NULL,
                           7
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           796,
                           15,
                           'RelateName',
                           '相关人姓名',
                           '领导姓名',
                           5,
                           NULL,
                           8
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           797,
                           15,
                           'Relation',
                           '关系',
                           '与领导关系',
                           7,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           798,
                           15,
                           'Number',
                           '编号',
                           '职务',
                           6,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           799,
                           67,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           800,
                           67,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           801,
                           67,
                           'Name',
                           '姓名',
                           '参保人姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           802,
                           67,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           803,
                           67,
                           'Addr',
                           '地址',
                           '参保人地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           804,
                           67,
                           'DataDate',
                           '日期',
                           '刷卡日期',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           805,
                           67,
                           'Amount',
                           '金额',
                           '金额',
                           5,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           806,
                           54,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           807,
                           54,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           808,
                           54,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           809,
                           54,
                           'RelateName',
                           '相关人姓名',
                           '相关人姓名',
                           2,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           810,
                           54,
                           'Name',
                           '姓名',
                           '姓名',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           811,
                           54,
                           'Addr',
                           '地址',
                           '地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           812,
                           54,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           813,
                           54,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           821,
                           63,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           822,
                           63,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           823,
                           63,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           824,
                           63,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           825,
                           63,
                           'Number',
                           '编号',
                           '编号',
                           4,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           826,
                           63,
                           'Addr',
                           '地址',
                           '地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           827,
                           63,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           828,
                           63,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           829,
                           64,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           830,
                           64,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           831,
                           64,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           832,
                           64,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           833,
                           64,
                           'Number',
                           '编号',
                           '编号',
                           4,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           834,
                           64,
                           'Addr',
                           '地址',
                           '地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           835,
                           64,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           836,
                           64,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           837,
                           70,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           838,
                           70,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           839,
                           70,
                           'Name',
                           '姓名',
                           '姓名',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           840,
                           70,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           841,
                           70,
                           'Addr',
                           '地址',
                           '单位',
                           2,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           842,
                           70,
                           'Number',
                           '编号',
                           '职务',
                           5,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           855,
                           71,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           856,
                           71,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           857,
                           71,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           858,
                           71,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           3,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           859,
                           71,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           860,
                           71,
                           'RelateID',
                           '相关人身份证号',
                           '相关人身份证号',
                           5,
                           NULL,
                           8
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           861,
                           71,
                           'RelateName',
                           '相关人姓名',
                           '相关人姓名',
                           6,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           862,
                           71,
                           'Relation',
                           '关系',
                           '关系',
                           8,
                           NULL,
                           10
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           863,
                           71,
                           'Number',
                           '编号',
                           '编号',
                           7,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           864,
                           17,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           865,
                           17,
                           'InputID',
                           '身份证号',
                           '家属身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           866,
                           17,
                           'Name',
                           '姓名',
                           '家属姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           867,
                           17,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           3,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           868,
                           17,
                           'RelateID',
                           '相关人身份证号',
                           '村干部身份证号',
                           5,
                           NULL,
                           7
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           869,
                           17,
                           'RelateName',
                           '相关人姓名',
                           '村干部姓名',
                           6,
                           NULL,
                           8
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           870,
                           17,
                           'Relation',
                           '关系',
                           '与村干部关系',
                           8,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           871,
                           17,
                           'Number',
                           '编号',
                           '职务',
                           7,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           872,
                           17,
                           'Addr',
                           '地址',
                           '单位',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           881,
                           19,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           882,
                           19,
                           'InputID',
                           '身份证号',
                           '法人身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           883,
                           19,
                           'Name',
                           '姓名',
                           '法人姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           884,
                           19,
                           'DataDate',
                           '日期',
                           '日期',
                           4,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           885,
                           19,
                           'Amount',
                           '金额',
                           '金额',
                           3,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           886,
                           19,
                           'RelateName',
                           '相关人姓名',
                           '公司名称',
                           5,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           903,
                           59,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           904,
                           59,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           905,
                           59,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           906,
                           59,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           907,
                           59,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           908,
                           59,
                           'DataDate',
                           '日期',
                           '刷卡日期',
                           8,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           909,
                           59,
                           'Amount',
                           '金额',
                           '刷卡金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           910,
                           59,
                           'RelateName',
                           '相关人姓名',
                           '医院',
                           9,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           911,
                           59,
                           'Number',
                           '编号',
                           '保险编号',
                           5,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           912,
                           21,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           913,
                           21,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           914,
                           21,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           915,
                           22,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           2,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           916,
                           22,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           917,
                           22,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           918,
                           22,
                           'RelateID',
                           '相关人身份证号',
                           '户主身份证号',
                           4,
                           NULL,
                           7
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           919,
                           22,
                           'RelateName',
                           '相关人姓名',
                           '户主姓名',
                           3,
                           NULL,
                           8
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           920,
                           22,
                           'Relation',
                           '关系',
                           '关系',
                           5,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           921,
                           51,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           922,
                           51,
                           'Name',
                           '姓名',
                           '项目名称',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           923,
                           51,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           924,
                           51,
                           'Addr',
                           '地址',
                           '地址',
                           2,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           925,
                           51,
                           'DataDate',
                           '日期',
                           '中标日期',
                           8,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           926,
                           51,
                           'AmountType',
                           '补助类型',
                           '项目类型',
                           5,
                           NULL,
                           7
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           927,
                           51,
                           'RelateName',
                           '相关人姓名',
                           '中标单位',
                           7,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           928,
                           51,
                           'Relation',
                           '关系',
                           '统一社会信用编码',
                           11,
                           NULL,
                           10
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           929,
                           51,
                           'Type',
                           '型号',
                           '资质要求',
                           6,
                           NULL,
                           11
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           930,
                           51,
                           'Number',
                           '编号',
                           '项目编号',
                           3,
                           NULL,
                           12
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           931,
                           51,
                           'Serial1',
                           '编号1',
                           '组织机构代码',
                           9,
                           NULL,
                           15
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           932,
                           51,
                           'Serial2',
                           '编号2',
                           '营业执照注册码',
                           10,
                           NULL,
                           16
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           933,
                           55,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           934,
                           55,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           935,
                           55,
                           'Name',
                           '姓名',
                           '姓名',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           936,
                           55,
                           'Addr',
                           '地址',
                           '地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           937,
                           55,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           938,
                           55,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           939,
                           55,
                           'RelateName',
                           '相关人姓名',
                           '学校名称',
                           2,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           940,
                           55,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           941,
                           56,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           942,
                           56,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           943,
                           56,
                           'Name',
                           '姓名',
                           '姓名',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           944,
                           56,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           945,
                           56,
                           'Addr',
                           '地址',
                           '地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           946,
                           56,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           947,
                           56,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           948,
                           56,
                           'RelateName',
                           '相关人姓名',
                           '学校名称',
                           2,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           949,
                           58,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           950,
                           58,
                           'InputID',
                           '身份证号',
                           '服务对象身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           951,
                           58,
                           'Name',
                           '姓名',
                           '服务对象姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           952,
                           58,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           953,
                           58,
                           'Addr',
                           '地址',
                           '服务对象地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           954,
                           58,
                           'DataDate',
                           '日期',
                           '服务时间',
                           6,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           955,
                           58,
                           'RelateID',
                           '相关人身份证号',
                           '医生身份证号',
                           8,
                           NULL,
                           8
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           956,
                           58,
                           'RelateName',
                           '相关人姓名',
                           '医生姓名',
                           7,
                           NULL,
                           9
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           957,
                           58,
                           'Type',
                           '型号',
                           '类型',
                           5,
                           NULL,
                           11
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           958,
                           25,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           959,
                           25,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           1,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           960,
                           25,
                           'Name',
                           '姓名',
                           '姓名',
                           2,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           961,
                           25,
                           'Amount',
                           '金额',
                           '金额',
                           4,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           962,
                           25,
                           'Type',
                           '型号',
                           '型号',
                           3,
                           NULL,
                           10
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           963,
                           60,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           964,
                           60,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           965,
                           60,
                           'Name',
                           '姓名',
                           '姓名',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           966,
                           60,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           967,
                           60,
                           'Addr',
                           '地址',
                           '地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           968,
                           60,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           969,
                           60,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           970,
                           60,
                           'Type',
                           '型号',
                           '职业介绍公司',
                           2,
                           NULL,
                           11
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           971,
                           61,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           972,
                           61,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           973,
                           61,
                           'Name',
                           '姓名',
                           '姓名',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           974,
                           61,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           975,
                           61,
                           'Addr',
                           '地址',
                           '地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           976,
                           61,
                           'DataDate',
                           '日期',
                           '开始日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           977,
                           61,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           978,
                           61,
                           'Type',
                           '型号',
                           '培训公司',
                           2,
                           NULL,
                           11
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           979,
                           61,
                           'DataDate1',
                           '日期1',
                           '结束日期',
                           8,
                           NULL,
                           14
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           980,
                           62,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           981,
                           62,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           3,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           982,
                           62,
                           'Name',
                           '姓名',
                           '姓名',
                           4,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           983,
                           62,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           984,
                           62,
                           'Addr',
                           '地址',
                           '地址',
                           5,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           985,
                           62,
                           'DataDate',
                           '日期',
                           '日期',
                           7,
                           NULL,
                           5
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           986,
                           62,
                           'Amount',
                           '金额',
                           '金额',
                           6,
                           NULL,
                           6
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           987,
                           62,
                           'DataDate1',
                           '日期1',
                           '日期1',
                           8,
                           NULL,
                           14
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           988,
                           62,
                           'Type',
                           '型号',
                           '培训公司',
                           2,
                           NULL,
                           11
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           989,
                           68,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           990,
                           68,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           991,
                           68,
                           'Name',
                           '姓名',
                           '姓名',
                           3,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           992,
                           68,
                           'Region',
                           '乡镇街道',
                           '乡镇街道',
                           1,
                           NULL,
                           3
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           993,
                           68,
                           'Addr',
                           '地址',
                           '地址',
                           4,
                           NULL,
                           4
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           994,
                           27,
                           'RowStart',
                           '起始行号',
                           '起始行号',
                           3,
                           NULL,
                           0
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           995,
                           27,
                           'InputID',
                           '身份证号',
                           '身份证号',
                           2,
                           NULL,
                           1
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           996,
                           27,
                           'Name',
                           '姓名',
                           '姓名',
                           1,
                           NULL,
                           2
                       );

INSERT INTO DataFormat (
                           RowID,
                           ParentID,
                           ColCode,
                           ColName,
                           DisplayName,
                           Col,
                           Comment,
                           Seq
                       )
                       VALUES (
                           997,
                           27,
                           'Type',
                           '型号',
                           '型号',
                           3,
                           NULL,
                           11
                       );


-- 表：DataItem
DROP TABLE IF EXISTS DataItem;

CREATE TABLE DataItem (
    RowID         INTEGER       PRIMARY KEY,
    ParentID      INTEGER,
    DataType      INTEGER,
    DataShortName VARCHAR (20)  NOT NULL,
    DataFullName  VARCHAR (40),
    DataLink      INTEGER,
    Datapath      VARCHAR (250),
    DataTime      VARCHAR (20),
    DBTable       VARCHAR (20),
    Status        BOOLEAN       DEFAULT true,
    Seq           INTEGER       NOT NULL
                                DEFAULT (999999),
    People        VARCHAR (10),
    dbTablePre    VARCHAR (20),
    Col1          VARCHAR (500),
    Col2          VARCHAR (500),
    UNIQUE (
        RowID
    )
);

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         1,
                         0,
                         0,
                         '资金比对',
                         '惠民政策监督检查',
                         0,
                         '',
                         '0001-01-01 00:00:00',
                         NULL,
                         1,
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         2,
                         1,
                         10,
                         '源数据',
                         '源数据',
                         0,
                         '\01源数据',
                         '0001-01-01 00:00:00',
                         NULL,
                         1,
                         1,
                         NULL,
                         NULL,
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         3,
                         1,
                         20,
                         '比对数据',
                         '比对数据',
                         0,
                         '\02比对数据',
                         '0001-01-01 00:00:00',
                         NULL,
                         1,
                         2,
                         NULL,
                         NULL,
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         5,
                         2,
                         30,
                         '农村五保',
                         '11农村五保户资金',
                         0,
                         '\01源数据\11农村五保户资金',
                         '0001-01-01 00:00:00',
                         'tbSourceFiveGuaranteFamily',
                         1,
                         110,
                         '吃',
                         'tbSourceFiveGuaranteFamilyPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         8,
                         3,
                         40,
                         '农村危房',
                         '22农村危房补助资金',
                         0,
                         '\02比对数据\22农村危房补助资金',
                         '0001-01-01 00:00:00',
                         'tbSourceRuralBadHouse',
                         1,
                         10220,
                         '领',
                         'tbSourceRuralBadHousePre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         10,
                         3,
                         40,
                         '农业补贴',
                         '17农业支持保护补贴资金',
                         0,
                         '\02比对数据\17农业支持保护补贴资金',
                         '0001-01-01 00:00:00',
                         'tbSourceFoodAid',
                         1,
                         10170,
                         '领农业补贴',
                         'tbSourceFoodAidPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         12,
                         3,
                         40,
                         '财政供养人员',
                         '01财政供养人员名单',
                         0,
                         '\02比对数据\01财政供养人员名单',
                         '0001-01-01 00:00:00',
                         'tbCompareCivilInfo',
                         1,
                         10010,
                         '财政供养人员',
                         'tbCompareCivilInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         14,
                         3,
                         40,
                         '领导干部',
                         '02领导干部名单',
                         0,
                         '\02比对数据\02领导干部名单',
                         '0001-01-01 00:00:00',
                         'tbCompareLeaderInfo',
                         1,
                         10020,
                         '领导干部',
                         'tbCompareLeaderInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         15,
                         3,
                         40,
                         '领导干部家属',
                         '03领导干部家属名单',
                         0,
                         '\02比对数据\03领导干部家属名单',
                         '0001-01-01 00:00:00',
                         'tbCompareLeaderRelateInfo',
                         1,
                         10030,
                         '领导家属',
                         'tbCompareLeaderRelateInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         16,
                         3,
                         40,
                         '村干部',
                         '04村干部名单',
                         0,
                         '\02比对数据\04村干部名单',
                         '0001-01-01 00:00:00',
                         'tbComparecountryInfo',
                         1,
                         10040,
                         '村干部',
                         'tbComparecountryInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         17,
                         3,
                         40,
                         '村干部家属',
                         '05村干部家属名单',
                         0,
                         '\02比对数据\05村干部家属名单',
                         '0001-01-01 00:00:00',
                         'tbComparecountryRelateInfo',
                         1,
                         10050,
                         '村家属',
                         'tbComparecountryRelateInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         18,
                         3,
                         40,
                         '纳税数据',
                         '06个人纳税数据',
                         0,
                         '\02比对数据\06个人纳税数据',
                         '0001-01-01 00:00:00',
                         'tbCompareIncomeTaxInfo',
                         1,
                         10060,
                         '有工作',
                         'tbCompareIncomeTaxInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         19,
                         3,
                         40,
                         '公司数据',
                         '07公司纳税数据',
                         0,
                         '\02比对数据\07公司纳税数据',
                         '0001-01-01 00:00:00',
                         'tbCompareCompanyInfo',
                         1,
                         10070,
                         '有公司',
                         'tbCompareCompanyInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         21,
                         3,
                         40,
                         '人口数据',
                         '08人口数据',
                         0,
                         '\02比对数据\08人口数据',
                         '0001-01-01 00:00:00',
                         'tbComparePersonInfo',
                         1,
                         10080,
                         '身份证姓名不一致',
                         'tbComparePersonInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         22,
                         3,
                         40,
                         '户籍数据',
                         '09户籍数据',
                         0,
                         '\02比对数据\09户籍数据',
                         '0001-01-01 00:00:00',
                         'tbCompareFamilyInfo',
                         1,
                         10090,
                         '直系亲属',
                         'tbCompareFamilyInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         23,
                         3,
                         40,
                         '车辆登记',
                         '10车辆登数据',
                         0,
                         '\02比对数据\10车辆登记数据',
                         '0001-01-01 00:00:00',
                         'tbCompareCarInfo',
                         1,
                         10100,
                         '有车',
                         'tbCompareCarInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         24,
                         3,
                         40,
                         '死亡名单',
                         '11死亡人员名单',
                         0,
                         '\02比对数据\11死亡人员名单',
                         '0001-01-01 00:00:00',
                         'tbCompareDeathInfo',
                         1,
                         10110,
                         '死亡超期',
                         'tbCompareDeathInfoPre',
                         '',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         25,
                         3,
                         40,
                         '购买农机',
                         '12农机购买数据',
                         0,
                         '\02比对数据\12农机购买户名单',
                         '0001-01-01 00:00:00',
                         'tbComparemachineInfo',
                         1,
                         10120,
                         '购买农机',
                         'tbComparemachineInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         26,
                         3,
                         40,
                         '火化登记',
                         '13火化登记数据',
                         0,
                         '\02比对数据\13火化登记',
                         '0001-01-01 00:00:00',
                         'tbCompareBurnInfo',
                         1,
                         10130,
                         '死亡超期',
                         'tbCompareBurnInfoPre',
                         '',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         27,
                         3,
                         40,
                         '房产数据',
                         '14房产登记数据',
                         0,
                         '\02比对数据\14房产数据',
                         '0001-01-01 00:00:00',
                         'tbCompareHouseInfo',
                         1,
                         10140,
                         '有房',
                         'tbCompareHouseInfoPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         35,
                         3,
                         40,
                         '生态公益林',
                         '19生态公益林补贴资金',
                         0,
                         '\02比对数据\19生态公益林补贴资金',
                         '0001-01-01 00:00:00',
                         'tbSourceForestEnvAid',
                         1,
                         10190,
                         '领',
                         'tbSourceForestEnvAidPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         36,
                         2,
                         30,
                         '农村低保',
                         '09农村居民最低生活保障资金',
                         0,
                         '\01源数据\09农村居民最低生活保障资金',
                         '0001-01-01 00:00:00',
                         'tbSourceSafeLowLifeContry',
                         1,
                         90,
                         '吃',
                         'tbSourceSafeLowLifeContryPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         37,
                         2,
                         30,
                         '城市低保',
                         '10城市居民最低生活保障资金',
                         0,
                         '\01源数据\10城市居民最低生活保障资金',
                         '0001-01-01 00:00:00',
                         'tbSourceSafeLowLifeCity',
                         1,
                         100,
                         '吃',
                         'tbSourceSafeLowLifeCityPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         40,
                         3,
                         40,
                         '退耕还林',
                         '18退耕还林补贴资金',
                         0,
                         '\02比对数据\18退耕还林补贴资金',
                         '0001-01-01 00:00:00',
                         'tbSourceForestAid',
                         1,
                         10180,
                         '领',
                         'tbSourceForestAidPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         43,
-                        1,
                         40,
                         '身份证未查到',
                         '身份证未查到',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         10,
                         '身份证未查到',
                         NULL,
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         44,
-                        1,
                         40,
                         '身份证姓名不一致',
                         '身份证姓名不一致',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         20,
                         '身份证姓名不一致',
                         NULL,
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         45,
-                        1,
                         40,
                         '面积大',
                         '核查面积大',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         30,
                         '面积大',
                         NULL,
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         46,
                         3,
                         40,
                         '工商登记',
                         '21工商登记数据',
                         0,
                         '\02比对数据\21工商登记',
                         '0001-01-01 00:00:00',
                         'tbCompareRegister',
                         1,
                         10210,
                         '有工商登记',
                         'tbCompareRegisterPre',
                         'update tbCompareRegister set RelateName = addr 
where RelateName is null',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         47,
                         3,
                         40,
                         '医保',
                         '15居民医疗保险数据',
                         0,
                         '\02比对数据\15居民医疗保险数据',
                         '0001-01-01 00:00:00',
                         'tbCompareMedical',
                         1,
                         10150,
                         '医保',
                         'tbCompareMedicalPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         48,
                         3,
                         40,
                         '入院信息',
                         '16入院信息',
                         0,
                         '\02比对数据\16入院信息',
                         '0001-01-01 00:00:00',
                         'tbCompareInHospital',
                         1,
                         10160,
                         '住院',
                         'tbCompareInHospitalPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         49,
                         3,
                         40,
                         '工程建设单位',
                         '20工程建设单位信息',
                         0,
                         '\02比对数据\20工程建设单位信息',
                         '0001-01-01 00:00:00',
                         'tbCompareBulidEnt',
                         1,
                         10200,
                         '资质',
                         'tbCompareBulidEntPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         50,
                         2,
                         30,
                         '贫困户',
                         '00贫困户信息',
                         0,
                         '\01源数据\00贫困户信息',
                         '0001-01-01 00:00:00',
                         'tbSourcePoorInfo',
                         1,
                         0,
                         '贫困户',
                         'tbSourcePoorInfoPre',
                         'delete from tbsourcePoorInfoPre;
INSERT INTO tbsourcePoorInfoPre (
                                    ID,
                                    sRelateID,
                                    sDataDate,
                                    InputID,
                                    Name,
                                    Region,
                                    Addr,
                                    DataDate,
                                    Amount,
                                    AmountType,
                                    RelateID,
                                    RelateName,
                                    Relation,
                                    Type,
                                    ItemType,
                                    Number,
                                    Area,
                                    DataDate1,
                                    Serial1,
                                    Serial2
                                )
                                SELECT tbsourcePoorInfo.ID,
                                       tbsourcePoorInfo.sRelateID,
                                       tbsourcePoorInfo.sDataDate,
                                       tbsourcePoorInfo.InputID,
                                       tbCompareFamilyInfo.Name,
                                       tbsourcePoorInfo.Region,
                                       tbsourcePoorInfo.Addr,
                                       tbsourcePoorInfo.DataDate,
                                       tbsourcePoorInfo.Amount,
                                       tbsourcePoorInfo.AmountType,
                                       tbsourcePoorInfo.RelateID,
                                       tbsourcePoorInfo.RelateName,
                                       tbsourcePoorInfo.Relation,
                                       tbsourcePoorInfo.Type,
                                       tbsourcePoorInfo.ItemType,
                                       tbsourcePoorInfo.Number,
                                       tbsourcePoorInfo.Area,
                                       tbsourcePoorInfo.DataDate1,
                                       tbsourcePoorInfo.Serial1,
                                       tbsourcePoorInfo.Serial2
                                  FROM tbsourcePoorInfo
                                       LEFT JOIN
                                       tbCompareFamilyInfo ON tbsourcePoorInfo.id = tbCompareFamilyInfo.id
                                 GROUP BY tbsourcePoorInfo.ID',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         51,
                         2,
                         30,
                         '整村推进',
                         '01整村推进项目信息',
                         0,
                         '\01源数据\01整村推进项目信息',
                         '0001-01-01 00:00:00',
                         'tbSourceVillageAdv',
                         1,
                         10,
                         '整村推进',
                         'tbSourceVillageAdvPre',
                         'update tbSourceVillageAdv set serial1 = ifnull(serial2, relation) where serial1 is null;
update tbSourceVillageAdv set serial2 = ifnull(Relation, serial1) where serial2 is null;
update tbSourceVillageAdv set Relation = ifnull(serial1, serial2) where Relation is null;
UPDATE tbSourceVillageAdv
   SET relation = relateName,
       serial1 = relateName,
       serial2 = relateName
 WHERE relation IS NULL AND 
       serial1 IS NULL AND 
       serial2 IS NULL',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         52,
                         2,
                         30,
                         '易地扶贫搬迁',
                         '02易地扶贫搬迁信息',
                         0,
                         '\01源数据\02易地扶贫搬迁信息',
                         '0001-01-01 00:00:00',
                         'tbSourceMigrate',
                         1,
                         20,
                         '易地扶贫搬迁',
                         'tbSourceMigratePre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         53,
                         2,
                         30,
                         '寄宿生补助',
                         '031义务教育阶段家庭经济困难寄宿生生活费补助',
                         0,
                         '\01源数据\031义务教育阶段家庭经济困难寄宿生生活费补助',
                         '0001-01-01 00:00:00',
                         'tbSourceBoardAid',
                         1,
                         31,
                         '领',
                         'tbSourceBoardAidPre',
                         '',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         54,
                         2,
                         30,
                         '高中助学金',
                         '032普通高中国家助学金',
                         0,
                         '\01源数据\032普通高中国家助学金',
                         '0001-01-01 00:00:00',
                         'tbSourceHighSchoolAd',
                         1,
                         32,
                         '领',
                         'tbSourceHighSchoolAdPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         55,
                         2,
                         30,
                         '中职助学金',
                         '033中等职业教育国家助学金',
                         0,
                         '\01源数据\033中等职业教育国家助学金',
                         '0001-01-01 00:00:00',
                         'tbSourceOccEduAd',
                         1,
                         33,
                         '领',
                         'tbSourceOccEduAdPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         56,
                         2,
                         30,
                         '雨露计划',
                         '034雨露计划',
                         0,
                         '\01源数据\034雨露计划',
                         '0001-01-01 00:00:00',
                         'tbSourceRainPlan',
                         1,
                         34,
                         '领',
                         'tbSourceRainPlanPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         57,
                         2,
                         30,
                         '扶贫信贷',
                         '04扶贫小额信贷资金',
                         0,
                         '\01源数据\04扶贫小额信贷资金',
                         '0001-01-01 00:00:00',
                         'tbSourceLoad',
                         1,
                         40,
                         '领',
                         'tbSourceLoadPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         58,
                         2,
                         30,
                         '公共卫生',
                         '05基本公共卫生服务补助',
                         0,
                         '\01源数据\05基本公共卫生服务补助',
                         '0001-01-01 00:00:00',
                         'tbSourceMedicalAd',
                         1,
                         50,
                         '领',
                         'tbSourceMedicalAdPre',
                         '',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         59,
                         2,
                         30,
                         '新农合',
                         '06新型农村合作医疗基金',
                         0,
                         '\01源数据\06新型农村合作医疗基金',
                         '0001-01-01 00:00:00',
                         'tbSourceVillageMedical',
                         1,
                         60,
                         '领',
                         'tbSourceVillageMedicalPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         60,
                         2,
                         30,
                         '职业介绍',
                         '071职业介绍补贴',
                         0,
                         '\01源数据\071职业介绍补贴',
                         '0001-01-01 00:00:00',
                         'tbSourceJobAd',
                         1,
                         71,
                         '领',
                         'tbSourceJobAdPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         61,
                         2,
                         30,
                         '职业培训',
                         '072职业培训补贴',
                         0,
                         '\01源数据\072职业培训补贴',
                         '0001-01-01 00:00:00',
                         'tbSourceJobTrainningAd',
                         1,
                         72,
                         '领',
                         'tbSourceJobTrainningAdPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         62,
                         2,
                         30,
                         '创业培训',
                         '073创业培训补贴',
                         0,
                         '\01源数据\073创业培训补贴',
                         '0001-01-01 00:00:00',
                         'tbSourceStartupAd',
                         1,
                         73,
                         '领',
                         'tbSourceStartupAdPre',
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         63,
                         2,
                         30,
                         '困难残疾人补贴',
                         '081困难残疾人生活补贴',
                         0,
                         '\01源数据\081困难残疾人生活补贴',
                         '0001-01-01 00:00:00',
                         'tbSourcePoorDisableAd',
                         1,
                         81,
                         '领',
                         'tbSourcePoorDisableAdPre',
                         '',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         64,
                         2,
                         30,
                         '重度残疾人补贴',
                         '082重度残疾人护理补贴',
                         0,
                         '\01源数据\082重度残疾人护理补贴',
                         '0001-01-01 00:00:00',
                         'tbSourceSevereDisableAd',
                         1,
                         82,
                         '领',
                         'tbSourceSevereDisableAdPre',
                         '',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         65,
-                        1,
                         40,
                         '贫困户低保五保',
                         '贫困户低保五保',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         40,
                         '无低保五保',
                         NULL,
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         66,
-                        1,
                         40,
                         '总金额大',
                         '核查总金额',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         30,
                         '总金额',
                         NULL,
                         NULL,
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         67,
                         3,
                         40,
                         '养老保险',
                         '23养老保险',
                         0,
                         '\02比对数据\23养老保险',
                         '0001-01-01 00:00:00',
                         'tbLifeInsurance',
                         0,
                         10230,
                         '领养老保险',
                         'tbLifeInsurancePre',
                         '',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         68,
                         2,
                         30,
                         '户户通',
                         '13户户通',
                         0,
                         '\01源数据\13户户通',
                         '0001-01-01 00:00:00',
                         'tbTV',
                         1,
                         130,
                         '享受',
                         'tbTVPre',
                         '',
                         NULL
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         69,
                         2,
                         30,
                         '扶贫到户',
                         '12扶贫到户资金',
                         0,
                         '\01源数据\12扶贫到户资金',
                         '0001-01-01 00:00:00',
                         'tbSourcePoorMoney',
                         0,
                         120,
                         '扶贫到户资金',
                         'tbSourcePoorMoneyPre',
                         '',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         70,
                         3,
                         40,
                         '五小所人员',
                         '24乡镇五小所人员名单',
                         0,
                         '\02比对数据\24乡镇五小所人员名单',
                         '0001-01-01 00:00:00',
                         'tbCompareTownFive',
                         1,
                         10240,
                         '五小所人员领',
                         'tbCompareTownFivePre',
                         '',
                         ''
                     );

INSERT INTO DataItem (
                         RowID,
                         ParentID,
                         DataType,
                         DataShortName,
                         DataFullName,
                         DataLink,
                         Datapath,
                         DataTime,
                         DBTable,
                         Status,
                         Seq,
                         People,
                         dbTablePre,
                         Col1,
                         Col2
                     )
                     VALUES (
                         71,
                         3,
                         40,
                         '五小所人员家属',
                         '25乡镇五小所人员家属名单',
                         0,
                         '\02比对数据\25乡镇五小所人员家属名单',
                         '0001-01-01 00:00:00',
                         'tbCompareTownFiveRelate',
                         1,
                         10250,
                         '五小所人员家属领',
                         'tbCompareTownFiveRelatePre',
                         '',
                         ''
                     );


-- 视图：vw_Region
DROP VIEW IF EXISTS vw_Region;
CREATE VIEW vw_Region AS
    SELECT B.RowID,
           A.RegionName ParentName,
           B.RegionName RegionName,
           B.RegionCode
      FROM HB_Region AS A
           INNER JOIN
           HB_Region AS B ON A.RowID = B.ParentID;


COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
