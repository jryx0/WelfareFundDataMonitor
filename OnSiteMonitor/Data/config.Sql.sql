--
-- ��SQLiteStudio v3.1.0 �������ļ� ��һ 2�� 27 11:15:08 2017
--
-- �ı����룺System
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- ��RulesTmp
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
                         'Դ��˫��һ�Զ�ģ��',
                         'SELECT ''@aimtype'' AS ����,
       @table1.Region AS ����ֵ�,
       @table1.ID AS ���֤��,
       @table2.Name AS @t2s����,
       @table2.Addr AS @t2s��ַ,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,
       @table1.sDataDate AS ��ȡʱ��,
       @table1.Amount AS ��ȡ���
  FROM @table1
       JOIN
      @table2 ON @table1.ID = @table2.ID
 WHERE  ((@table1.ID) = 15 OR 
          length(@table1.ID) = 18)  and  @table1.Amount>= @para
 ORDER BY ����ֵ�,@t1s��ַ,
          ���֤��,��ȡʱ�� 
 
',
                         'SELECT distinct ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,       
       group_concat(distinct @t2s����) @t2s����,  
        @t1s��ַ,
       min(date(��ȡʱ��) ) ��ʼ��ȡʱ��,     
       max(date(��ȡʱ��) ) �����ȡʱ��,
       sum(��ȡ���) ���,       
       group_concat(distinct @t2s��ַ) ��ע       
  FROM @tablename
 GROUP BY ����,���֤��           
 ORDER BY ����ֵ�,@t1s��ַ,���֤��
',
                         'SELECT distinct ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,
       @t1s��ַ,  
       min(��ȡʱ��) ||'' �� '' || max(��ȡʱ��) ��ȡʱ��,
      ''@t2s��ַ:''  || ifnull(group_concat(distinct @t2s��ַ), '''') ��ע @injection  
  FROM @tablename
 GROUP BY ����,         
          ����ֵ�,
          ���֤��           
 ORDER BY ����ֵ�,@t1s��ַ,���֤��',
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
                         'Դ��˫���Զ�ģ��',
                         'SELECT DISTINCT ''@aimtype'' AS ����,
                @table1.Region AS ����ֵ�,
                @table1.ID AS ���֤��,
                @table1.Name AS @t1s����,
                b.Name AS @t2s����,
                @table1.Addr AS @t1s��ַ,
                @table1.sDataDate AS ��ȡʱ��,
                @table1.Amount AS ��ȡ���,
                b.�ͺ� AS ��ע
  FROM @table1,
       (SELECT DISTINCT ID,
                        Name,
                        group_concat(Type, ''/'') AS �ͺ�
          FROM @table2
         GROUP BY ID,
                  Name) b
 WHERE ( (length(@table1.ID) = 15 OR 
          length(@table1.ID) = 18) ) AND 
       b.ID = @table1.ID
 ORDER BY ����ֵ�,@t1s��ַ,
          ���֤�� ,��ȡʱ��',
                         'SELECT distinct ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,  
       group_concat(distinct @t2s����) @t2s����,
       @t1s��ַ,
       min(date(��ȡʱ��) ) ��ʼ��ȡʱ��,     
       max(date(��ȡʱ��) ) �����ȡʱ��,
       sum(��ȡ���) ���,
       ��ע     
  FROM @tablename
 GROUP BY ����,���֤��,��ע
 ORDER BY ����ֵ�,@t1s��ַ,���֤��',
                         'SELECT distinct ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,  
       @t1s��ַ,
       min(��ȡʱ��) ||'' �� '' || max(��ȡʱ��) ��ȡʱ��,
        ''@t2s����:'' || @t2s���� || '','' || ifnull(substr(��ע, 1, 100), '''') ��ע @injection 
  FROM @tablename
 GROUP BY ����,���֤��,��ע
 ORDER BY ����ֵ�,@t1s��ַ,���֤��

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
                         'Դ�ȱ�������ģ��',
                         'SELECT ''@aimtype'' AS ����,
       @table1.Region AS ����ֵ�,
       @table1.ID AS ���֤��,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,
       @table1.sDataDate AS ��ȡʱ��,
       @table1.Amount AS ��ȡ���,
       b.ID AS @t2s���֤��,
       b.Name AS @t2s����,
       b.��ע AS ��ע,
       @table3.Relation AS ������ϵ
  FROM @table1,
       @table3,
       (SELECT ID,
               Name,
               group_concat(Type, ''/'') AS ��ע
          FROM @table2 where (length(ID) = 15 OR 
        length(ID) = 18)
         GROUP BY ID,
                  Name) b
 WHERE @table3.sRelateID = @table1.ID AND 
       (@table3.Relation IN (''��'',
       ''��'',
       ''��ż'',
       ''��'',
       ''Ů'',
       ''����'',
       ''��Ů'',
       ''����'',
       ''��Ů'',
       ''����'',
       ''��Ů'',
       ''��'',
       ''ĸ'',
       ''��ĸ'') ) AND 
       @table3.ID =  b.ID AND 
       (length(@table1.ID) = 15 OR 
        length(@table1.ID) = 18) 
 ORDER BY ����ֵ�,@t1s��ַ,
         ���֤��,��ȡʱ��',
                         'SELECT DISTINCT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s����,      
      group_concat(distinct @t2s����) @t2s����,
      @t1s��ַ,
      group_concat(distinct @t2s���֤��) @t2s���֤��,
      min(date(��ȡʱ��) ) ��ʼ��ȡʱ��,     
      max(date(��ȡʱ��) ) �����ȡʱ��,
      sum(��ȡ���) ���,
      group_concat(distinct ������ϵ) ������ϵ ,
      group_concat(distinct ��ע) ��ע
  FROM @tablename
 GROUP BY ����,          
          ���֤��
 ORDER BY ����ֵ�,@t1s��ַ,
          ���֤��
',
                         'SELECT DISTINCT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s����,  
      @t1s��ַ,    
      min(��ȡʱ��) ||'' �� '' || max(��ȡʱ��) ��ȡʱ��,  
     ''@t2s����:''|| ifnull(group_concat(distinct @t2s����), '''') ||
      ifnull( group_concat(distinct @t2s���֤��), '''')  || 
     '',��ϵ:''||   ifnull(group_concat(distinct ������ϵ), '''') || 
     '', '' ||  ifnull(group_concat(distinct ��ע), '''')  ��ע @injection 
  FROM @tablename
 GROUP BY ����,          
          ���֤��
 ORDER BY ����ֵ�,@t1s��ַ,
          ���֤��',
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
                         '���������һ��ģ��',
                         'SELECT DISTINCT ''@aimtype'' AS ����,
                @table1.Region AS ����ֵ�,
                @table1.ID AS ���֤��,
                @table1.Name AS @t1s����,
                @table1.Addr AS @t1s��ַ,
                tbComparePersonInfo.Name AS ��������,
                @table1.sDataDate AS ��ȡʱ��,
                @table1.Amount AS ��ȡ���,
                @table1.Type AS ��ע
  FROM @table1
       JOIN
       tbComparePersonInfo ON @table1.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> @table1.Name AND 
       (length(@table1.ID) = 15 OR 
        length(@table1.ID) = 18) 
 ORDER BY ����ֵ�,          
          @t1s��ַ,
          ���֤��,��ȡʱ��',
                         'SELECT DISTINCT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s���� ,
      group_concat(distinct ��������) �������� ,
      @t1s��ַ, 
      min(date(��ȡʱ��) ) ��ʼ��ȡʱ��,     
      max(date(��ȡʱ��) ) �����ȡʱ��,
      sum(��ȡ���) ��� ,
      ifnull(group_concat(��ע), '''') as ��ע   
  FROM @tablename
 GROUP BY ����,           
          ���֤��           
 ORDER BY ����ֵ�,
          @t1s��ַ,
          ���֤��',
                         'SELECT DISTINCT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s���� ,      
      @t1s��ַ, 
      min(��ȡʱ��) ||'' �� '' || max(��ȡʱ��) ��ȡʱ��,
      ''��������:'' || ifnull(group_concat(distinct ��������), '''') || '','' || ifnull(��ע, '''')  as ��ע  
     @injection 
  FROM @tablename
 GROUP BY ����,           
          ���֤��           
 ORDER BY ����ֵ�,
          @t1s��ַ,
          ���֤��

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
                         '����ģ��',
                         'SELECT ����,
       ����ֵ�,
       ���֤��,
       @t1s����,
       @t1s��ַ,
       ��ȡʱ��,
       ��ȡ���,
       �������֤��,
       ��������,
       ����ʱ��
  FROM (SELECT ''@aimtype'' AS ����,
               @table1.Region AS ����ֵ�,
               @table1.ID AS ���֤��,
               @table1.Name AS @t1s����,
               @table1.Addr AS @t1s��ַ,
               @table1.sDataDate AS ��ȡʱ��,
               @table1.Amount AS ��ȡ���,
               tbCompareBurnInfo.InputID AS �������֤��,
               tbCompareBurnInfo.Name AS ��������,
               tbCompareBurnInfo.sDataDate AS ����ʱ��
          FROM @table1
               JOIN
               tbCompareBurnInfo ON @table1.ID = tbCompareBurnInfo.ID
         WHERE tbCompareBurnInfo.sDataDate < @table1.sDataDate AND 
               (length(@table1.ID) = 15 OR 
                length(@table1.ID) = 18) 
       UNION
       SELECT ''@aimtype'' AS ����,
              @table1.Region AS ����ֵ�,
              @table1.ID AS ���֤��,
              @table1.Name AS @t1s����,
              @table1.Addr AS @t1s��ַ,
              @table1.sDataDate AS ��ȡʱ��,
              @table1.Amount AS ��ȡ���,
              tbCompareDeathInfo.InputID AS �������֤��,
              tbCompareDeathInfo.Name AS ��������,
              tbCompareDeathInfo.sDataDate AS ����ʱ��
         FROM @table1
              JOIN
              tbCompareDeathInfo ON @table1.ID = tbCompareDeathInfo.ID
        WHERE tbCompareDeathInfo.sDataDate < @table1.sDataDate AND 
              (length(@table1.ID) = 15 OR 
               length(@table1.ID) = 18) 
        ORDER BY ����ֵ�,
                 ���֤��) 
 WHERE ���֤�� IN (SELECT DISTINCT ���֤��
                  FROM (SELECT @table1.ID AS ���֤��,
                               @table1.sDataDate AS ��ȡʱ��,
                               tbCompareBurnInfo.sDataDate AS ����ʱ��
                          FROM @table1
                               JOIN
                               tbCompareBurnInfo ON @table1.ID = tbCompareBurnInfo.ID
                         WHERE tbCompareBurnInfo.sDataDate < @table1.sDataDate AND 
                               (length(@table1.ID) = 15 OR 
                                length(@table1.ID) = 18) 
                       UNION
                       SELECT @table1.ID AS ���֤��,
                              @table1.DataDate AS ��ȡʱ��,
                              tbCompareDeathInfo.sDataDate AS ����ʱ��
                         FROM @table1
                              JOIN
                              tbCompareDeathInfo ON @table1.ID = tbCompareDeathInfo.ID
                        WHERE tbCompareDeathInfo.sDataDate < @table1.sDataDate AND 
                              (length(@table1.ID) = 15 OR 
                               length(@table1.ID) = 18) ) 
                 WHERE julianday(��ȡʱ��) - julianday(����ʱ��) > @para AND 
               ����ʱ�� > ''2010-01-01'')  
order by ����ֵ�,
       ���֤�� ,��ȡʱ��',
                         'SELECT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s���� ,
      @t1s��ַ, 
      min(date(��ȡʱ��) ) ��ʼ��ȡʱ��,     
      max(date(��ȡʱ��) ) �����ȡʱ��,
      sum(��ȡ���) ��� ,
      group_concat(distinct �������֤��) �������֤��,
      group_concat(distinct  ��������) ��������,
      max(����ʱ��),
      CAST (julianday(��ȡʱ��) - julianday(����ʱ��) AS INT) AS ��������
  FROM @tablename
 WHERE �������� > @para
 GROUP BY ����,
          ����ֵ�,
          ���֤��
 ORDER BY ����ֵ�,
          �������� DESC,
          ���֤��',
                         'SELECT DISTINCT ����,
                ����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ,
                min(��ȡʱ��) || '' �� '' || max(��ȡʱ��) ��ȡʱ��,
                sum(��ȡ���) �ܽ�� @injection
  FROM @tablename
 GROUP BY ����,@t1s����,
          ���֤��
 ORDER BY ����ֵ�,
         @t1s��ַ,
          ���֤��',
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
                         '���֤δ�鵽ģ��',
                         'SELECT DISTINCT ''@aimtype'' AS ����,
                @table1.Region AS ����ֵ�,
                @table1.ID AS ���֤��,
                @table1.Addr AS @t1s��ַ,
                @table1.Name AS @t1s����,
                @table1.sDataDate AS ��ȡʱ��,
                @table1.Amount AS ��ȡ���,
                @table1.Type AS ��ע
  FROM @table1
 WHERE @table1.ID NOT IN (SELECT tbComparePersonInfo.ID
                                              FROM tbComparePersonInfo) AND 
       (length(@table1.ID) = 15 OR 
        length(@table1.ID) = 18) 
 ORDER BY ����ֵ�,
          @t1s��ַ,
          ���֤��,��ȡʱ��',
                         'SELECT DISTINCT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s���� ,
      @t1s��ַ,       
      min(date(��ȡʱ��) ) ��ʼ��ȡʱ��,     
      max(date(��ȡʱ��) ) �����ȡʱ��,
      sum(��ȡ���) ��� ,
      ifnull(group_concat(��ע), '''')  as ��ע      
  FROM @tablename
 GROUP BY ����,          
          ���֤��            
 ORDER BY ����ֵ�,@t1s��ַ, ���֤��',
                         'SELECT DISTINCT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s���� ,
      @t1s��ַ,       
      min(��ȡʱ��) || '' �� '' || max(��ȡʱ��) ��ȡʱ��,
      ifnull(group_concat(��ע), '''') as ��ע              @injection 
  FROM @tablename
 GROUP BY ����,          
          ���֤��            
 ORDER BY ����ֵ�,@t1s��ַ, ���֤��
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
                         'Դ�ȱ�����ģ��',
                         'SELECT ''@aimtype'' AS ����,
       @table1.region AS ����ֵ�,      
       @table1.ID AS ���֤��,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,
       @table1.sdataDate AS ��ȡʱ��,
       @table1.Amount AS ��ȡ���,
       a.sRelateID AS @t3s���֤��,
       a.RelateName AS @t3s����,
       a.Relation AS ��ϵ
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
 ORDER BY ����ֵ�,
          @t1s��ַ,
          ���֤��,
          ��ȡʱ��',
                         'SELECT ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,      
       @t1s��ַ,
       min(date(��ȡʱ��) ) ��ʼ��ȡʱ��,     
      max(date(��ȡʱ��) ) �����ȡʱ��,
      sum(��ȡ���) ���,    
       ''@t3s���֤��:'' ||ifnull(@t3s���֤��, '''') || '',@t3s����:'' || ifnull(@t3s����, '''') || '','' || ifnull(��ϵ, '''')    as ��ע
  FROM @tablename
 GROUP BY ����,
       ���֤��,
       @t3s����,      
      @t3s���֤��,        
       ��ϵ 
 ORDER BY ����ֵ�,���֤��',
                         'SELECT ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,      
       @t1s��ַ,
       min(��ȡʱ��) ||'' �� '' || max(��ȡʱ��) ��ȡʱ��,        
       ''@t3s���֤��:'' ||ifnull(@t3s���֤��, '''') || '',@t3s����:'' || ifnull(@t3s����, '''') || '',��ϵ:'' || ifnull(��ϵ, '''')    as ��ע @injection 
  FROM @tablename
 GROUP BY ����,
       ���֤��,
       @t3s����,      
      @t3s���֤��,        
       ��ϵ 
 ORDER BY ����ֵ�,���֤��
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
                         'ԴԴ˫��ʱ���ж�ģ��',
                         'SELECT ''@aimtype'' ����,
       a.����ֵ�  ����ֵ�,
       a.���֤�� ���֤��,
       a.@t1s���� @t1s����,
       a.@t1s��ַ @t1s��ַ,
       a.@t1s��ȡ��� @t1s��ȡ���,
       a.@t1s��ȡ��� @t1s���,        
       strftime(''%Y'', a.@t1s��ȡ���) || ''��:'' || b.@t2s��� || ''Ԫ''  ��ע
  FROM
       (SELECT DISTINCT group_concat(DISTINCT @table1.Region) AS ����ֵ�,
                        @table1.ID AS ���֤��,
                        group_concat(DISTINCT @table1.Name) AS @t1s����,
                        group_concat(DISTINCT @table1.Addr) AS @t1s��ַ,
                        date(@table1.sDataDate, ''start of year'') AS @t1s��ȡ���,
                        sum(@table1.Amount) as @t1s��ȡ���
          FROM @table1
         GROUP BY ���֤��,
                  @t1s��ȡ���) a left join
       (SELECT DISTINCT @table2.ID AS ���֤��,
                        date(@table2.sDataDate, ''start of year'') AS @t2s��ȡ���,
                        sum(@table2.Amount) AS @t2s���
          FROM @table2
         WHERE (length(@table2.ID) = 15 OR 
                length(@table2.ID) = 18)  
               
         GROUP BY ���֤��,
                  @t2s��ȡ���) b on  a.���֤�� = b.���֤��
 WHERE  
       a.@t1s��ȡ��� = b.@t2s��ȡ��� AND 
       a.@t1s��ȡ��� > date(''2013-01-01'') AND 
       b.@t2s��ȡ��� > date(''2013-01-01'') and b.@t2s��� > @para
 ORDER BY a.����ֵ�, a.���֤��',
                         'SELECT DISTINCT ����,
                ����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ ,                
                group_concat( ��ע, ''��'') as ��ע
  FROM @tablename
group by ����,
                ����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ',
                         'SELECT DISTINCT ����,                
                group_concat(distinct ����ֵ�) ����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) @t1s����,      
                @t1s��ַ,
                @t1s��ȡ��� ��ȡʱ��,
                group_concat( ��ע, ''��'') as ��ע @injection 
  FROM @tablename
group by ����,
                ����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ',
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
                         'ԴԴ˫��ʱ���ж�ģ��-2',
                         'SELECT DISTINCT ''@aimtype'' AS ����,
                a.����ֵ� as ����ֵ�,
                a.���֤�� as ���֤��,
                a.���� AS @t1s����,
                a.��ַ as @t1s��ַ,
                a.������ȡʱ�� AS @t1s������ȡʱ��,
                a.������ȡʱ�� AS @t1s������ȡʱ��,
                a.��� AS @t1s��ȡ���,
                b.������ȡʱ�� AS @t2s������ȡʱ��,
                b.������ȡʱ�� AS @t2s������ȡʱ��,
                b.��� AS @t2s��ȡ���
  FROM (
           SELECT group_concat(DISTINCT @table1.Region) AS ����ֵ�,
                  @table1.ID AS ���֤��,
                  group_concat(DISTINCT @table1.Name) AS ����,
                  @table1.addr AS ��ַ,
                  min(date(@table1.sDataDate) ) AS ������ȡʱ��,
                  max(date(@table1.sDataDate) ) AS ������ȡʱ��,
                  sum(@table1.Amount) AS ���
             FROM @table1
            WHERE length(id) > 1
            GROUP BY ���֤��
       )
       a,
       (
           SELECT group_concat(DISTINCT @table2.Region) AS ����ֵ�,
                  @table2.ID AS ���֤��,
                  group_concat(DISTINCT @table2.Name) AS ����,
                  min(date(@table2.sDataDate) ) AS ������ȡʱ��,
                  max(date(@table2.sDataDate) ) AS ������ȡʱ��,
                  sum(@table2.Amount) AS ���
             FROM @table2
            WHERE length(id) > 1
            GROUP BY ���֤��
       )
       b
 WHERE a.���֤�� = b.���֤�� AND 
       (@t1s������ȡʱ�� BETWEEN @t2s������ȡʱ�� AND @t2s������ȡʱ�� OR 
        @t1s������ȡʱ�� BETWEEN @t2s������ȡʱ�� AND @t2s������ȡʱ��) AND 
       @t2s��ȡ��� > 0
 ORDER BY ����ֵ�,
          ���֤��;
',
                         'SELECT DISTINCT ����,
                group_concat(distinct ����ֵ�) ����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����)  @t1s����,
                @t1s��ַ,
                ''@t1s��ȡʱ��:''||@t1s������ȡʱ�� ||'' �� ''|| @t1s������ȡʱ�� || '', @t2s��ȡʱ��: ''  ||  @t2s������ȡʱ�� || '' �� ''|| @t2s������ȡʱ�� as ��ȡʱ��,
                sum(@t1s��ȡ���) + sum(@t2s��ȡ���)  ��ע  
  FROM @tablename
 GROUP BY ����,@t1s����,
          ���֤��
 ORDER BY ����ֵ�,
          @t1s��ַ,
          ���֤��',
                         'SELECT DISTINCT ����,
                group_concat(distinct ����ֵ�) ����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����)  @t1s����,
                @t1s��ַ,
                ''@t1s��ȡʱ��:''||@t1s������ȡʱ�� ||'' �� ''|| @t1s������ȡʱ�� || '', @t2s��ȡʱ��: ''  ||  @t2s������ȡʱ�� || '' �� ''|| @t2s������ȡʱ�� as ��ȡʱ��,
                ''�ܽ��:'' || (sum(@t1s��ȡ���) + sum(@t2s��ȡ���))  ��ע @injection
  FROM @tablename
 GROUP BY ����,@t1s����,
          ���֤��
 ORDER BY ����ֵ�,
          @t1s��ַ,
          ���֤��',
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
                         '��������˫�ֶ��ظ�����',
                         'SELECT ''@aimtype'' AS ����,
       tbSourceMedicalAd.Region ����ֵ�,
       tbSourceMedicalAd.ID ���֤��,
       tbSourceMedicalAd.Name ����,
       tbSourceMedicalAd.Addr ��ַ,
       tbSourceMedicalAd.Type  ����,
       tbSourceMedicalAd.sRelateID ҽ�����֤ ,
       tbSourceMedicalAd.RelateName  ҽ�� ,
       tbSourceMedicalAd.DataDate ����ʱ��  
  FROM tbSourceMedicalAd
 WHERE ID IN (
           SELECT ID
             FROM tbSourceMedicalAd
            GROUP BY ID 
           HAVING count(id) >= @para
       )
 ORDER BY srelateid,
          ID',
                         'SELECT DISTINCT ����,
                group_concat(DISTINCT ����ֵ�) ����ֵ�,
                ���֤��,
                group_concat(DISTINCT ����) @t1s����,
                ��ַ,
                min(date(����ʱ��) ) ��ʼ����ʱ��,     
               max(date(����ʱ��) ) ������ʱ��,
                ''ҽ��:'' || group_concat(DISTINCT ҽ��) || '', ����:'' ||group_concat(DISTINCT ����)     ��ע
  FROM @tablename        
 GROUP BY ����,
          ���֤��
 ORDER BY ����ֵ�,
          ���֤��',
                         'SELECT DISTINCT ����,
                group_concat(DISTINCT ����ֵ�) ����ֵ�,
                ���֤��,
                group_concat(DISTINCT ����) @t1s����,
                ��ַ,
                min(date(����ʱ��) ) ||''-'' || max(date(����ʱ��) )  ��ȡʱ��,
                  ''ҽ��:'' || group_concat(DISTINCT ҽ��) || '', ����:'' ||group_concat(DISTINCT ����)     ��ע @injection 
  FROM @tablename        
 GROUP BY ����,
          ���֤��
 ORDER BY ����ֵ�,
          ���֤��',
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
                         'ԴԴ˫��ʱ���ж�ģ��-1',
                         'SELECT DISTINCT ''@aimtype'' AS ����,
                a.����ֵ�,
                a.���֤��,
                a.���� AS @t1s����,
                a.@t1s��ַ,
                a.@t1s������ȡʱ�� AS ���й���������ȡʱ��,
                a.@t1s������ȡʱ�� AS ���й���������ȡʱ��,
                a.@t1s��� AS ������ȡ���,
                b.@t2s������ȡʱ�� AS ��ɢ����������ȡʱ��,
                b.@t2s������ȡʱ�� AS ��ɢ����������ȡʱ��,
                b.@t2s��� AS ��ɢ��ȡ���
  FROM
       (SELECT DISTINCT @table1.Region AS ����ֵ�,
                        @table1.ID AS ���֤��,
                        @table1.Name AS ����,
                        @table1.addr AS @t1s��ַ,
                        min(date(@table1.sDataDate) ) AS @t1s������ȡʱ��,
                        max(date(@table1.sDataDate) ) AS @t1s������ȡʱ��,
                        sum(@table1.Amount) AS @t1s���
          FROM @table1 where @table1.AmountType like ''%��ɢ%''
         GROUP BY ����ֵ�,
                  ����,
                  ���֤��) a,
       (SELECT DISTINCT @table2.Region AS ����ֵ�,
                        @table2.ID AS ���֤��,
                        @table2.Name AS ����,
                        min(date(@table2.sDataDate) ) AS @t2s������ȡʱ��,
                        max(date(@table2.sDataDate) ) AS @t2s������ȡʱ��,
                        sum(@table2.Amount) AS @t2s���
          FROM @table2 where @table2.AmountType like ''%����%''
         GROUP BY ����ֵ�,
                  ����,
                  ���֤��) b
 WHERE a.���֤�� = b.���֤�� AND 
       (length(a.���֤��) = 15 OR 
        length(a.���֤��) = 18) AND 
       (a.@t1s������ȡʱ�� BETWEEN b.@t2s������ȡʱ�� AND b.@t2s������ȡʱ�� OR 
        a.@t1s������ȡʱ�� BETWEEN b.@t2s������ȡʱ�� AND b.@t2s������ȡʱ��)
 ORDER BY a.����ֵ�,
          a.���֤��
',
                         'SELECT DISTINCT ����,
                group_concat(distinct ����ֵ�)as ����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) as @t1s����,
                @t1s��ַ,   
                ''���й���:'' || ���й���������ȡʱ�� || ''��'' || ���й���������ȡʱ�� || 
                '', ��ɢ����:'' || ��ɢ����������ȡʱ�� || ''��'' || ��ɢ����������ȡʱ�� as ��ע
  FROM @tablename
group by ���֤��,���й���������ȡʱ��, ���й���������ȡʱ��,��ɢ����������ȡʱ��,��ɢ����������ȡʱ��',
                         'SELECT DISTINCT ����,
                group_concat(distinct ����ֵ�)as ����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) as @t1s����,
                @t1s��ַ,                   
                ''���й���:'' || ���й���������ȡʱ�� || ''��'' || ���й���������ȡʱ�� || 
                '', ��ɢ����:'' || ��ɢ����������ȡʱ�� || ''��'' || ��ɢ����������ȡʱ�� ��ȡʱ��,
                '''' ��ע @injection 
  FROM @tablename
group by ���֤��,���й���������ȡʱ��, ���й���������ȡʱ��,��ɢ����������ȡʱ��,��ɢ����������ȡʱ��',
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
                         '�ص���Ⱥ�ȶ�ģ��(�޽��)',
                         'SELECT ''@aimtype'' AS ����,
       @table1.Region AS ����ֵ�,
       @table1.ID AS ���֤��,
       @table2.Name AS @t2s����,
       @table2.Addr AS @t2s��λ,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,
       @table1.sDataDate AS ��ȡʱ��,
       @table1.Amount AS ��ȡ���
  FROM @table1
       JOIN
      @table2 ON @table1.ID = @table2.ID
 WHERE  ((@table1.ID) = 15 OR 
          length(@table1.ID) = 18) 
 ORDER BY ����ֵ�,@t1s��ַ,
          ���֤��,��ȡʱ��
',
                         'SELECT distinct ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,       
       group_concat(distinct @t2s����) @t2s����,  
        @t1s��ַ,
       min(date(��ȡʱ��) ) ��ʼ��ȡʱ��,     
       max(date(��ȡʱ��) ) �����ȡʱ��,
       sum(��ȡ���) ���,       
       group_concat(distinct @t2s��λ) ��ע       
  FROM @tablename
 GROUP BY ����,���֤��           
 ORDER BY ����ֵ�,@t1s��ַ,���֤��',
                         'SELECT distinct ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,
       @t1s��ַ,  
       min(��ȡʱ��) ||'' �� '' || max(��ȡʱ��) ��ȡʱ��,
      ''@t2s��λ:''  || ifnull(group_concat(distinct @t2s��λ), '''') ��ע @injection  
  FROM @tablename
 GROUP BY ����,         
          ����ֵ�,
          ���֤��           
 ORDER BY ����ֵ�,@t1s��ַ,���֤��',
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
                         '�ص���Ⱥ�����ȶ�ģ��(�޽��)',
                         'SELECT ''@aimtype'' AS ����,
       @table1.region AS ����ֵ�,      
       @table1.ID AS ���֤��,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,
       
       a.sRelateID AS @t3s���֤��,
       a.RelateName AS @t3s����,
       a.Relation AS ��ϵ
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
 ORDER BY ����ֵ�,
          @t1s��ַ,
          ���֤��',
                         'SELECT ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,      
       group_concat(distinct @t1s��ַ) @t1s��ַ,    
       ''@t3s���֤��:'' ||ifnull(@t3s���֤��, '''') || '',@t3s����:'' || ifnull(@t3s����, '''') || '','' || ifnull(��ϵ, '''')    as ��ע
  FROM @tablename
 GROUP BY ����,
       ���֤��
 ORDER BY ����ֵ�,���֤��',
                         'SELECT ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,      
       @t1s��ַ,
       '''' ��ȡʱ��,        
       ''@t3s���֤��:'' ||ifnull(@t3s���֤��, '''') || '',@t3s����:'' || ifnull(@t3s����, '''') || '',��ϵ:'' || ifnull(��ϵ, '''')    as ��ע @injection 
  FROM @tablename
 GROUP BY ����,
       ���֤��,
       @t3s����,      
      @t3s���֤��,        
       ��ϵ 
 ORDER BY ����ֵ�,���֤��
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
                         '���������֤(��ʱ��)',
                         'select  ''@aimtype'' AS ����,
               @table1.Region AS ����ֵ�,
               @table1.ID AS ���֤��,
               @table1.Name AS @t1s����,
               @table1.Addr AS @t1s��ַ,
               
               tbCompareDeathInfo.InputID AS �������֤��,
               tbCompareDeathInfo.Name AS �������� 
               from @table1 join tbCompareDeathInfo   
on tbCompareDeathInfo.ID = @table1.ID
where  length( @table1.ID )>1
union
select  ''@aimtype'' AS ����,
               @table1.Region AS ����ֵ�,
               @table1.ID AS ���֤��,
               @table1.Name AS @t1s����,
               @table1.Addr AS @t1s��ַ,
               
               tbCompareBurnInfo.InputID AS �������֤��,
               tbCompareBurnInfo.Name AS �������� 
               from @table1 join tbCompareBurnInfo   
on tbCompareBurnInfo.ID = @table1.ID
where  length( @table1.ID )>1',
                         'SELECT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s���� ,
      @t1s��ַ, 
       
      group_concat(distinct �������֤��) �������֤��,
      group_concat(distinct  ��������) �������� 
     
  FROM @tablename
  
 GROUP BY ����,
           
          ���֤��
 ORDER BY ����ֵ�,
        
          ���֤��',
                         'SELECT DISTINCT ����,
                 group_concat(distinct ����ֵ�) ����ֵ�,
                 ���֤��,
                 group_concat(distinct @t1s����) @t1s���� ,
                 @t1s��ַ, 
                '''' ��ȡʱ��,
                ''�������֤��:'' || group_concat(distinct �������֤��) ||  ''��������:'' ||
      group_concat(distinct  ��������)  ��ע @injection
  FROM @tablename
 GROUP BY ����,@t1s����,
          ���֤��
 ORDER BY ����ֵ�,
         @t1s��ַ,
          ���֤��',
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
                         'ƶ���������ж�',
                         'select ����,����ֵ�,���֤��,@t1s����,@t1s��ַ,��ȡʱ��,�������,��ע from 

(
SELECT ''@aimtype'' AS ����,
       @tablepre1.Region AS ����ֵ�,
       @tablepre1.ID AS ���֤��,
       group_concat(DISTINCT @tablepre1.Name) AS @t1s����,
       group_concat(DISTINCT @tablepre1.Addr) AS @t1s��ַ,
       @table2.DataDate AS ��ȡʱ��,
       sum(@table2.Amount) AS �������,
       '''' AS ��ע
  FROM    @tablepre1
       JOIN
       @table2 ON @tablepre1.id = @table2.id
 WHERE  
        length( @tablepre1.ID )>1
group by @table2.id)
where ������� >= @para',
                         'SELECT DISTINCT ����,
                ����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ ,                
                group_concat( ��ע, ''��'') as ��ע
  FROM @tablename
group by ����,
                ����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ',
                         'SELECT DISTINCT ����,                
                group_concat(distinct ����ֵ�) ����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) @t1s����,      
                @t1s��ַ,
                ��ȡʱ��,
                group_concat( ��ע, ''��'') as ��ע @injection 
  FROM @tablename
group by ����,
                ����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ',
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
                         'һ����������һ������',
                         'SELECT ''@aimtype'' AS ����,
       @table1.Region AS @t1s����ֵ�,
       @table1.ID AS ���֤��,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,       
       @table1.sDataDate AS @t1sʱ�� 
  FROM @table1
 WHERE (
           SELECT count(1) AS num
             FROM @tablepre2
            WHERE @table1.ID = @tablepre2.ID
       ) =  0 and (length(@table1.ID) > 0)',
                         'SELECT DISTINCT ����,
                group_concat(distinct @t1s����ֵ�) @t1s����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) @t1s����,
                group_concat(distinct @t1s��ַ)  @t1s��ַ,   
                 count(@t1sʱ��) ���Ŵ���,
                '''' as ��ע
  FROM @tablename
group by ����,           
                ���֤��
              ',
                         'SELECT DISTINCT ����,                
                group_concat(distinct @t1s����ֵ�) @t1s����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) @t1s����,      
                @t1s��ַ,
                @t1sʱ��,
                ''''  ��ע @injection 
  FROM @tablename
group by ����,
                @t1s����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ',
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
                         '�ظ���ȡ����',
                         'SELECT  ''@aimtype'' AS ����,
       @table1.Region as @t1s����ֵ�,
       @table1.ID as ���֤��,
       group_concat(DISTINCT @table1.Name) AS @t1s����,
       group_concat(DISTINCT @table1.Addr) AS @t1s��ַ,
       count(@table1.DataDate) AS @t1s����,
        group_concat(@table1.Area)  ��ע
  FROM @table1
 WHERE  length( @table1.ID )>1
 GROUP BY id
HAVING count(id) > @para
',
                         'SELECT DISTINCT ����,
                @t1s����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ ,   
                 @t1s����,
                ��ע as ��ע
  FROM @tablename
group by ����,
                @t1s����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ
order by @t1s����ֵ�,@t1s����,@t1s��ַ ,  ���֤��',
                         'SELECT DISTINCT ����,                
                group_concat(distinct @t1s����ֵ�) @t1s����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) @t1s����,      
                @t1s��ַ,
                @t1s����,
                ��ע ��ע @injection 
  FROM @tablename
group by ����,
                @t1s����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ
order by @t1s����ֵ�,@t1s����,@t1s��ַ ,  ���֤��',
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
                         '˫���ѯ��ͬ����',
                         'SELECT ''@aimtype'' AS ����,
       @table1.Region AS @t1s����ֵ�,
       @table1.ID AS ���֤��,
       @table2.Name AS @t2s����,
       @table2.Addr AS @t2s��ַ,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,
       @table1.sDataDate AS @t1sʱ��
  FROM @table1
       JOIN
      @table2 ON @table1.ID = @table2.ID
 WHERE   length( @table1.ID )>1  
 ORDER BY @t1s����ֵ�,@t1s��ַ,
          ���֤��,@t1sʱ�� ',
                         'SELECT distinct ����,
       group_concat(distinct @t1s����ֵ�) @t1s����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,       
       group_concat(distinct @t2s����) @t2s����,  
        @t1s��ַ,
       min(date(@t1sʱ��) ) ��ʼʱ��,     
       max(date(@t1sʱ��) ) ���ʱ��,
       '''' ���,       
       group_concat(distinct @t2s��ַ) ��ע       
  FROM @tablename
 GROUP BY ����,���֤��           
 ORDER BY @t1s����ֵ�,@t1s��ַ,���֤��',
                         'SELECT distinct ����,
       group_concat(distinct @t1s����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,
       @t1s��ַ,  
       min(@t1sʱ��) ||'' �� '' || max(@t1sʱ��) ��ȡʱ��,
      ''@t2s��ַ:''  || ifnull(group_concat(distinct @t2s��ַ), '''') ��ע @injection  
  FROM @tablename
 GROUP BY ����,         
          
          ���֤��           
 ORDER BY ����ֵ�,@t1s��ַ,���֤��',
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
                         '�������',
                         'SELECT ''@aimtype'' AS ����,    
       @table1.Region AS @t1s����ֵ�,
       @table1.ID AS ���֤��,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,       
       @table1.sDataDate AS @t1sʱ��,    
       @table1.Amount as ��ͥ�˿�, 
@table1.Area as ���,  
       @table1.Area/@table1.Amount as ƽ�����
  FROM @table1
  where @table1.Amount > 0 and 
     length(@table1.ID) > 1 @para
order by @t1s����ֵ�,@t1s��ַ,   ���֤�� ',
                         'SELECT distinct ����,
       group_concat(distinct  @t1s����ֵ�) @t1s����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,  
        @t1s��ַ,
        date(@t1sʱ��) ��ʼʱ��,     
       '''' ���ʱ��,
       ��ͥ�˿� �˿�,       
       ''ƽ�����:''|ƽ����� ��ע       
  FROM @tablename
 GROUP BY ����,���֤��           
 ORDER BY  @t1s����ֵ�,@t1s��ַ,���֤��',
                         'SELECT distinct ����,
       group_concat(distinct @t1s����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,
       @t1s��ַ,  
       max(@t1sʱ��) ��ȡʱ��,
       ''ƽ�����:'' ||ƽ����� || '',�˿ڣ�'' ||  ��ͥ�˿� || '',���:'' || ���  ��ע @injection  
  FROM @tablename
 GROUP BY ����,  
          ���֤��           
 ORDER BY ����ֵ�,@t1s��ַ,���֤��',
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
                         '���֤�����ж�',
                         'SELECT DISTINCT ''@aimtype'' AS ����,
                @table1.Region AS ����ֵ�,
                @table1.ID AS ���֤��,
                @table1.Addr AS @t1s��ַ,
                @table1.Name AS @t1s����,
                @table1.sDataDate AS @t1s��ȡʱ��,
                @table1.Amount AS @t1s��ȡ���,
                datadate - datetime(substr(id, 7, 4) || ''-'' || substr(id, 11, 2) || ''-'' || substr(id, 13, 2) ) AS ����,
                @table1.Type as ��ע
  FROM @table1
 WHERE   @para',
                         'SELECT distinct ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,     
        @t1s��ַ,
       group_concat(@t1s��ȡʱ��) @t1s��ȡʱ��,            
       sum(@t1s��ȡ���) ���,       
       ''����:''||����|| '',''|| ��ע as ��ע       
  FROM @tablename
 GROUP BY ����,���֤��           
 ORDER BY ����ֵ�,@t1s��ַ,���֤��, @t1s��ȡʱ��,��ע 
',
                         'SELECT distinct ����,
       group_concat(distinct ����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,
       @t1s��ַ,  
      group_concat(@t1s��ȡʱ��) ��ȡʱ��,
       ''���:'' ||@t1s��ȡ���|| '',����:'' ||  ����  ��ע @injection  
  FROM @tablename
 GROUP BY ����,  
          ���֤��           
 ORDER BY ����ֵ�,@t1s��ַ,���֤��,��ע',
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
                         '����',
                         '
SELECT ''@aimtype'' AS ����,
       @table1.Region AS @t1s����ֵ�,
       @table1.ID AS ���֤��,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,
       @table1.sDataDate AS @t1sʱ��,
       @table1.Amount AS ���         
  FROM @table1 ��
where  length( @table1.ID )>1
 GROUP BY @table1.Amount, strftime(''%Y'', @table1.sDataDate) 
HAVING (sum(@table1.Amount)  @para ) 
 ORDER BY @t1s����ֵ�, @t1s��ַ,���֤��',
                         'SELECT distinct ����,
       group_concat(distinct @t1s����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,  
       group_concat(distinct @t2s����) @t2s����,
       @t1s��ַ,
       min(date(@t1sʱ��) ) ��ʼ��ȡʱ��,     
       max(date(@t1sʱ��) ) �����ȡʱ��,
       sum(���) ���,
       '''' ��ע     
  FROM @tablename
 GROUP BY ����,���֤��, @t1s��ַ,��ע
 ORDER BY ����ֵ�,@t1s��ַ,���֤��',
                         'SELECT distinct ����,
       group_concat(distinct @t1s����ֵ�) ����ֵ�,
       ���֤��,
       group_concat(distinct @t1s����) @t1s����,
       @t1s��ַ,  
       ''��ʼ��ȡʱ��:'' || min(date(@t1sʱ��) )  ||''�����ȡʱ��:''  ||   
       max(date(@t1sʱ��) )   ��ȡʱ��,
       ''���:'' || sum(���)  ��ע @injection  
  FROM @tablename
 GROUP BY ����,  @t1s��ַ,  
          ���֤��           
 ORDER BY ����ֵ�,@t1s��ַ,���֤��',
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
                         '��ƶ�Ŵ���ҵ���',
                         'SELECT ''@aimtype'' AS ����,
       @table1.Region AS @t1s����ֵ�,
      @table1.ID AS ���֤��,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,
       @table1.sDataDate AS @t1sʱ��,
       @table1.Amount AS ���
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
                         'SELECT DISTINCT ����,
                @t1s����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) @t1s����,      
                group_concat(distinct @t1s��ַ ) ��ַ,  
                sum(���)             
  FROM @tablename
group by ����,
                @t1s����ֵ�,
                ���֤�� 
               
order by @t1s����ֵ�, ��ַ ,  ���֤��',
                         'SELECT DISTINCT ����,                
                group_concat(distinct @t1s����ֵ�) @t1s����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) @t1s����,      
                 group_concat(distinct @t1s��ַ ) ��ַ,  
                ''''       ʱ��,
                sum(���) as ��ע @injection 
  FROM @tablename
group by ����,
                @t1s����ֵ�,
                ���֤��,
                @t1s����,
                @t1s��ַ',
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
                         'ͬ��˫ʱ���ж�',
                         'SELECT ''@aimtype'' AS ����,
       tbSourceMedicalAd.Region AS ����ֵ�,
       tbSourceMedicalAd.ID AS  ���֤��,
tbSourceMedicalAd.Name AS  @t1s����,
       tbSourceMedicalAd.Addr AS @t1s��ַ,
       tbSourceMedicalAd.sDataDate AS ʱ��,
       tbSourceMedicalAd.RelateID AS ҽ�����֤��,
       tbSourceMedicalAd.RelateName AS ҽ������,
       tbCompareInHospital.DataDate AS ��Ժʱ��,
       tbCompareInHospital.DataDate1 AS ��Ժʱ��
  FROM tbSourceMedicalAd
       JOIN
       tbCompareInHospital ON tbSourceMedicalAd.id = tbCompareInHospital.id
 WHERE tbSourceMedicalAd.sDataDate BETWEEN datetime(tbCompareInHospital.DataDate) AND datetime(tbCompareInHospital.dataDate1)
and length(tbSourceMedicalAd.ID) > 1',
                         'SELECT DISTINCT ����,
                ����ֵ�,
                ���֤��,
                group_concat(DISTINCT ������������) ������������,
                group_concat(DISTINCT ����������ַ) ��ַ,
                ʱ�� ����ʱ��,
                ҽ������,
                ҽ�����֤��,
                ��Ժʱ��,
                ��Ժʱ��
  FROM @tablename
group by  
                ���֤��    
order by ����ֵ�,  ���֤��',
                         'SELECT DISTINCT ����,
                ����ֵ�,
                ���֤��,
                group_concat(DISTINCT ������������) ������������,
                group_concat(DISTINCT ����������ַ) ��ַ,
                ʱ�� ʱ��,
                ''ҽ�����֤�ţ�'' || ҽ�����֤�� || '' ��ҽ��������'' || ҽ������ || ''��Ժʱ��:'' || ��Ժʱ�� || ''-��Ժʱ��:'' || ��Ժʱ�� AS ��ע  @injection 
  FROM @tablename
 GROUP BY ���֤��
 ORDER BY ����ֵ�,
          ��ַ,
          ���֤��',
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
                         'һ�����һ������-2',
                         'SELECT ''@aimtype'' AS ����,
       @table1.Region AS @t1s����ֵ�,
       @table1.ID AS ���֤��,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,       
       @table1.sDataDate AS ����ʱ��,       
       @table1.Amount as ���Ž��
  FROM @table1
 WHERE (
           SELECT count(1) AS num
             FROM @table2
            WHERE @table1.ID = @table2.ID
       ) =  0 and (length(@table1.ID) > 0)',
                         'SELECT DISTINCT ����,
                group_concat(distinct @t1s����ֵ�) @t1s����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) @t1s����,
                group_concat(distinct @t1s��ַ)  @t1s��ַ,   
                 count(����ʱ��) ���Ŵ���,
                ''���: '' ||  ���Ž�� as ��ע
  FROM @tablename
group by ����,                
                ���֤��
order by @t1s����ֵ� 
               ',
                         'SELECT DISTINCT ����,                
                group_concat(distinct @t1s����ֵ�) @t1s����ֵ�,
                ���֤��,
                group_concat(distinct @t1s����) @t1s����,      
                @t1s��ַ,
                min(����ʱ��) || '' �� '' || max(����ʱ��) ��ȡʱ��,
                ''���:'' || sum(���Ž��) as ��ע @injection 
  FROM @tablename
group by ����,               
                ���֤��,                
                @t1s��ַ
order by @t1s����ֵ�',
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
                         '�����ظ�����',
                         'SELECT ''@aimtype'' AS ����,
       @table1.Region ����ֵ�,
       @table1.ID ���֤��,
       @table1.Name ����,
       @table1.Addr ��ַ,
       @table1.Type ��˾,
       @table1.sDataDate ʱ��,
       b.DataDateYear ���
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
                         'SELECT DISTINCT ����,
                ����ֵ�,
                ���֤��,
                group_concat(distinct ����) ����,      
                group_concat(distinct ��ַ ) ��ַ,  
                count(ʱ��)    ����,
                ���,
                  group_concat(distinct        ��˾) ��˾
  FROM @tablename
group by ����,
                 
                ���֤��   
order by ����ֵ�, ��ַ ,  ���֤��',
                         'SELECT DISTINCT ����,
                ����ֵ�,
                ���֤��,
                group_concat(distinct ����) ����,      
                group_concat(distinct ��ַ ) ��ַ,  
               ��� ʱ��,
               ''����:'' || count(���֤��)  as ��ע  @injection 
  FROM @tablename
group by ����,
                 
                ���֤�� 
order by ����ֵ�, ��ַ ,  ���֤��',
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
                         '�����ƽ���Ŀ�б���Ŀ����',
                         'SELECT ''@aimtype'' AS ����,
                  tbSourceVillageAdv.serial2 Ӫҵִ����,
                  tbSourceVillageAdv.relatename ��˾����,
                  tbSourceVillageAdv.region ����ֵ�,
                  tbSourceVillageAdv.number ��Ŀ���,
                  tbSourceVillageAdv.name ��Ŀ����,
                  tbSourceVillageAdv.addr ��Ŀ��ַ,
                  tbSourceVillageAdv.serial1 ��֯������,
                  tbSourceVillageAdv.Relation ͳһ������
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
            ORDER BY ����ֵ�,tbSourceVillageAdv.serial1 DESC',
                         'SELECT ����,
       Ӫҵִ����,
       ��֯������,
       ͳһ������,
       ��˾����,
       count(��Ŀ����) �б�����   
  FROM @tablename
GROUP BY ��֯������          
 ORDER BY �б����� desc',
                         'SELECT DISTINCT ����,
                ����ֵ�,
                ��֯������ as ��֯������_Ӫҵִ��_ͳһ������,
                ��˾����,
                '''' ��ַ,
                '''' ʱ��,
                ''�б����:'' || count(��Ŀ����) AS ��ע  @injection 
  FROM @tablename
GROUP BY ����,
          ��֯������_Ӫҵִ��_ͳһ������
 ORDER BY count(��Ŀ����) desc;',
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
                         '�����ƽ���Ŀ�б���Ŀ����(������)',
                         'SELECT 
       ''@aimtype'' as ����,
       tbSourceVillageAdv.serial2 Ӫҵִ����,
       tbSourceVillageAdv.relatename ��˾����,
       tbSourceVillageAdv.region ����ֵ�,
       tbSourceVillageAdv.number ��Ŀ���,
       tbSourceVillageAdv.name ��Ŀ����,
       tbSourceVillageAdv.addr ��Ŀ��ַ,
       tbSourceVillageAdv.serial1 ��֯������,
       tbSourceVillageAdv.Relation ͳһ������
  FROM tbSourceVillageAdv
 WHERE serial2 IN (
           SELECT serial2
             FROM tbSourceVillageAdv
            GROUP BY serial2,region
            ORDER BY count() DESC
            LIMIT @para
       )
 ORDER BY tbSourceVillageAdv.serial2 DESC,
          ����ֵ�',
                         'SELECT ����, ����ֵ�, Ӫҵִ����,
       ��˾����,
       count(��Ŀ����) �б�����
  FROM @tablename
GROUP BY Ӫҵִ����,����ֵ�,
          ��˾����
 ORDER BY �б����� desc',
                         'SELECT DISTINCT ����,
                ����ֵ�,
                Ӫҵִ���� ���֤��,
                ��˾���� ����,      
                '''' ��ַ,  
               '''' ʱ��,
               ''�б����:'' || count(��Ŀ����)  as ��ע  @injection 
  FROM @tablename
group by ����,
                ����ֵ�,
                ���֤�� 
order by ���֤��,����ֵ�',
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
                         '��ͥ�������',
                         'SELECT ''@aimtype'' AS ����,
       @table1.Region AS ����ֵ�,
       @table1.ID AS ���֤��,
       @table1.Name AS @t1s����,
       @table1.Addr AS @t1s��ַ,
       @table1.sDataDate AS ��ȡʱ��,
       @table1.Amount AS ��ȡ���,
       a.��ע
  FROM (
           SELECT ���֤��,
                  ''��Ʒ������:'' || sum(����) || '','' || group_concat(��ע) AS ��ע
             FROM (
                      SELECT DISTINCT @table1.ID AS ���֤��,
                                      b.����,
                                      '' ���:'' || b.�ͺ� AS ��ע
                        FROM @table1,
                             (
                                 SELECT DISTINCT ID,
                                                 Name,
                                                 group_concat(Type, ''/'') AS �ͺ�,
                                                 count() ����
                                   FROM @table2
                                  WHERE length(ID) > 1
                                  GROUP BY ID,
                                           Name
                             )
                             b
                       WHERE length(@table1.ID) > 1 AND 
                             b.ID = @table1.ID
                       GROUP BY ���֤��
                      UNION ALL
                      SELECT @table1.ID AS ���֤��,
                             b.����,
                             ''���:'' || b.��ע || '',������ϵ:'' || @table3.Relation || '',����ID:'' || @table3.RelateID AS ��ע
                        FROM @table1,
                             @table3,
                             (
                                 SELECT ID,
                                        Name,
                                        group_concat(Type, ''/'') AS ��ע,
                                        count() ����
                                   FROM @table2
                                  WHERE length(ID) > 1
                                  GROUP BY ID,
                                           Name
                             )
                             b
                       WHERE @table3.sRelateID = @table1.ID AND 
                             (@table3.Relation IN (''��'', ''��'', ''��ż'', ''��'', ''Ů'', ''����'', ''��Ů'', ''����'', ''��Ů'', ''����'', ''��Ů'', ''��'', ''ĸ'', ''��ĸ'') ) AND 
                             @table3.ID = b.ID AND 
                             length(@table1.ID) > 1
                       GROUP BY ���֤��
                  )
            GROUP BY ���֤��
           HAVING sum(����) > @para
       )
       a
       JOIN
       @table1 ON a.���֤�� = @table1.id
 WHERE length(@table1.id) > 1
 ORDER BY @table1.Region,
          @table1.id;',
                         'SELECT DISTINCT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s����,
      min(date(��ȡʱ��) ) ��ʼ��ȡʱ��,     
      max(date(��ȡʱ��) ) �����ȡʱ��,
      sum(��ȡ���) ���,
      group_concat(distinct ��ע) ��ע
  FROM @tablename
 GROUP BY ����,          
          ���֤��
 ORDER BY ����ֵ�,@t1s��ַ,
          ���֤��
',
                         'SELECT DISTINCT ����,
      group_concat(distinct ����ֵ�) ����ֵ�,
      ���֤��,
      group_concat(distinct @t1s����) @t1s����,  
      @t1s��ַ,    
      min(��ȡʱ��) ||'' �� '' || max(��ȡʱ��) ��ȡʱ��,  
     ''��ȡ���:'' || sum(��ȡ���) || '','' || ��ע  @injection 
  FROM @tablename
 GROUP BY ����,          
          ���֤��
 ORDER BY ����ֵ�,@t1s��ַ,
          ���֤��',
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
                         'ʱ���쳣',
                         'SELECT  ''@aimtype'' as ����,
       @table1.Region ����ֵ�,
       @table1.ID ���֤��,
       @table1.Name ����,
       @table1.Addr ��ַ,
       @table1.type ��˾,
       datadate1,
       datadate,
       julianday(datadate1) - julianday(sdatadate) ��ѵʱ��
  FROM @table1
 WHERE ��ѵʱ�� < 7 OR 
       ��ѵʱ�� > 365',
                         'SELECT  ''@aimtype'' as ����,
       @table1.Region ����ֵ�,
       @table1.ID ���֤��,
       @table1.Name ����,
       @table1.Addr ��ַ,
       @table1.type ��˾,
      ��ѵʱ��
from @tablename
order by ��˾,���֤��',
                         'SELECT    ����,
         ����ֵ�,
        ���֤��,
         ����,
         ��ַ,
      
      ��ѵʱ��,
     ''��˾:'' || @table1.type as ��ע  @injection 
from @tablename
order by ��˾,���֤��',
                         '',
                         2,
                         1,
                         9999999
                     );


-- ��DataLabel
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


-- ��Setting
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


-- ��User
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


-- ��Task
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
                     '��1�δ����ݱȶ�',
                     '2017-02-22 12:41:04',
                     '����ʡ2017��02��22��12��41��03�룬���о�׼��ƶ������ʵ����������ݷ����ȶԡ�',
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
                     '��2�δ����ݱȶ�',
                     '2017-02-22 13:02:26',
                     '����ʡ2017��02��22��13��02��25�룬���о�׼��ƶ������ʵ����������ݷ����ȶԡ�',
                     '',
                     '67698146955.029',
                     0,
                     10,
                     0,
                     'admin'
                 );


-- ��HB_Region
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
                          '����ʡ',
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
                          'Ǳ����',
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
                          '�人��',
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
                          '��ʯ��',
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
                          'ʮ����',
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
                          '������',
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
                          '�˲���',
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
                          '������',
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
                          '������',
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
                          '������',
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
                          'Т����',
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
                          '�Ƹ���',
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
                          '������',
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
                          '������',
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
                          '��ʩ����������������',
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
                          '��ũ������',
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
                          '������',
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
                          '������',
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
                          '������',
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
                          '������',
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
                          '�~����',
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
                          '������',
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
                          '�����',
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
                          '��ɽ��',
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
                          '��ɽ��',
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
                          '�̵���',
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
                          '������',
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
                          '��������',
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
                          '������',
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
                          '������',
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
                          '������',
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
                          '����������',
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
                          '���ÿ�����',
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
                          '�����羰��',
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
                          '�人������',
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
                          '��ұ��',
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
                          '������',
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
                          '��ʯ����',
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
                          '����ɽ��',
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
                          '��½��',
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
                          '��ɽ��',
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
                          '���ü���������',
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
                          '�¸�������ҵ԰��',
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
                          '��������',
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
                          '������',
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
                          '������',
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
                          '����',
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
                          '��ɽ��',
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
                          '��Ϫ��',
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
                          'é����',
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
                          '������',
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
                          '�䵱ɽ����',
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
                          '���ü���������',
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
                          '������',
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
                          '�˳���',
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
                          '������',
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
                          '������',
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
                          '�ȳ���',
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
                          '�Ϻӿ���',
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
                          '�����',
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
                          '������',
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
                          '������',
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
                          '������',
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
                          '������',
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
                          '�����޾��ÿ�����',
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
                          '�˶���',
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
                          '֦����',
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
                          '������',
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
                          'Զ����',
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
                          '��ɽ��',
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
                          '������',
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
                          '������',
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
                          '���������������',
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
                          '������',
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
                          '������',
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
                          '��Ҹ���',
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
                          '�����',
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
                          '�Vͤ��',
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
                          'ʯ����',
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
                          '������',
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
                          'ɳ����',
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
                          '������',
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
                          '������',
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
                          '������',
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
                          '������',
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
                          '�����',
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
                          '��ɽ��',
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
                          'ɳ����',
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
                          '������',
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
                          '������',
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
                          '�޵���',
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
                          'ʯ����',
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
                          '������',
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
                          '������',
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
                          '���Ӻ���',
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
                          'Т����',
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
                          '������',
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
                          'Ӧ����',
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
                          '������',
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
                          '��½��',
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
                          '������',
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
                          'Т����',
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
                          '������',
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
                          '�ŷ���',
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
                          '�찲��',
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
                          '�����',
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
                          '������',
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
                          'Ӣɽ��',
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
                          '�ˮ��',
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
                          'ޭ����',
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
                          '��÷��',
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
                          '���к�������',
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
                          '�̰���',
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
                          '������',
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
                          '�����',
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
                          'ͨ����',
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
                          '������',
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
                          'ͨɽ��',
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
                          '����',
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
                          '��ˮ��',
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
                          '������',
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
                          '��ʩ��',
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
                          '������',
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
                          '��ʼ��',
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
                          '�Ͷ���',
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
                          '������',
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
                          '������',
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
                          '�̷���',
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
                          '�׷���',
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


-- ��CompareAim
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
                           '��ɲ���ũ��ͱ�',
                           '��ɲ�����ũ��ͱ�����',
                           '��ɲ���ũ��ͱ�',
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
                           '�쵼�ɲ���ũ��ͱ�',
                           '�쵼�ɲ�����ũ��ͱ�����',
                           '�쵼�ɲ���ũ��ͱ�',
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
                           '����������Ա��ũ��ͱ�',
                           '����������Ա����ũ��ͱ�����',
                           '����������Ա��ũ��ͱ�',
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
                           '����������Ա�Գ��еͱ�',
                           '����������Ա���ǳ��еͱ�����',
                           '����������Ա�Գ��еͱ�',
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
                           '�쵼�ɲ��Գ��еͱ�',
                           '�쵼�ɲ����ǳ��еͱ�����',
                           '�쵼�ɲ��Գ��еͱ�',
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
                           '��ɲ��Գ��еͱ�',
                           '��ɲ����ǳ��еͱ�����',
                           '��ɲ��Գ��еͱ�',
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
                           '����������Ա��ũ���屣',
                           '����������Ա����ũ���屣����',
                           '����������Ա��ũ���屣',
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
                           '�쵼�ɲ���ũ���屣',
                           '�쵼�ɲ�����ũ���屣����',
                           '�쵼�ɲ���ũ���屣',
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
                           '��ɲ���ũ���屣',
                           '��ɲ�����ũ���屣����',
                           '��ɲ���ũ���屣',
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
                           '�й�����ũ��ͱ�',
                           '��ʵ�й�����ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�й�����ũ��ͱ�',
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
                           '�г���ũ��ͱ�',
                           '��ʵ�г���ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�г���ũ��ͱ�',
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
                           '����ũ����ũ��ͱ�',
                           '��ʵ����ũ����ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '����ũ����ũ��ͱ�',
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
                           '�˲�ֱϵ�����й�����ũ��ͱ�',
                           '��ʵֱϵ�����й�����ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����й�����ũ��ͱ�',
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
                           '�˲�ֱϵ�����г���ũ��ͱ�',
                           '��ʵֱϵ�����г���ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����г���ũ��ͱ�',
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
                           '�й����Գ��еͱ�',
                           '��ʵ�й����Գ��еͱ���Ա��ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�й����Գ��еͱ�',
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
                           '�й�˾�Գ��еͱ�',
                           '��ʵ�й�˾�Գ��еͱ���Ա��ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�й�˾�Գ��еͱ�',
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
                           '�г��Գ��еͱ�',
                           '��ʵ�г��Գ��еͱ���Ա��ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�г��Գ��еͱ�',
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
                           '����ũ���Գ��еͱ�',
                           '��ʵ����ũ���ͱ���Ա��ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '����ũ���Գ��еͱ�',
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
                           '�з���ũ��ͱ�',
                           '��ʵ�з���ũ��ͱ���Ա��ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�з���ũ��ͱ�',
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
                           '�й�����ũ���屣',
                           '��ʵ�й�����ũ���屣��Ա��ͥ���������ȷ���Ƿ����ũ���屣����',
                           '�й�����ũ���屣',
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
                           '����ũ����ũ���屣',
                           '��ʵ����ũ����ũ���屣��Ա��ͥ���������ȷ���Ƿ����ũ���屣����',
                           '����ũ����ũ���屣',
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
                           '�з���ũ���屣',
                           '��ʵ�з���ũ���屣��Ա��ͥ���������ȷ���Ƿ����ũ���屣����',
                           '�з���ũ���屣',
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
                           '�г���ũ���屣',
                           '��ʵ�г���ũ���屣��Ա��ͥ���������ȷ���Ƿ����ũ���屣����',
                           '�г���ũ���屣',
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
                           '�й�˾��ũ���屣',
                           '��ʵ�й�˾��ũ���屣��Ա��ͥ���������ȷ���Ƿ����ũ���屣����',
                           '�й�˾��ũ���屣',
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
                           '�й�˾��ũ��ͱ�',
                           '��ʵ�й�˾��ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�й�˾��ũ��ͱ�',
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
                           '�˲�ֱϵ�����й�˾��ũ��ͱ�',
                           '��ʵֱϵ�����й�˾��ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����й�˾��ũ��ͱ�',
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
                           '�˲�ֱϵ��������ũ����ũ��ͱ�',
                           '��ʵֱϵ��������ũ����ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ��������ũ����ũ��ͱ�',
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
                           '�˲�ֱϵ�����з���ũ��ͱ�',
                           '��ʵֱϵ�����з���ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����з���ũ��ͱ�',
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
                           '��������ũ��ͱ�',
                           '�˲�������Ա���ڳ�ũ��ͱ����',
                           '��������90�컹��ũ��ͱ�',
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
                           'ũ��ͱ����֤������һ��',
                           '�ȽϹ����˿���Ϣ���������֤һ�¶�������һ�µĵͱ���Ա��ȷ����ͱ���Ա������Ƿ���ȷ',
                           'ũ��ͱ����֤������һ��',
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
                           'ũ��ͱ����֤δ�鵽',
                           '�ȽϹ����˿���Ϣ��δ�鵽ũ��ͱ������֤��Ϣ��ȷ����ͱ���Ա������Ƿ���ȷ',
                           'ũ��ͱ����֤δ�鵽',
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
                           '�쵼�ɲ�������ũ��ͱ�',
                           '�˲��쵼�ɲ�������ũ��ͱ������ȷ�����ͥ�����Ƿ�������߹涨',
                           '�쵼�ɲ�������ũ��ͱ�',
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
                           '��ɲ�������ũ��ͱ�',
                           '�˲��ɲ�������ũ��ͱ������ȷ�����ͥ�����Ƿ�������߹涨',
                           '��ɲ�������ũ��ͱ�',
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
                           '���˸����ֲ��������ֳ�ũ��ͱ�',
                           '��ʵ���˸����ֲ�������ļ�ͥ���������ȷ���Ƿ����ũ��ͱ�����',
                           '���˸����ֲ����ֳ�ũ��ͱ�',
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
                           '����̬�����ֲ��������ֳ�ũ��ͱ�',
                           '��ʵ����̬�����ֲ�������ļ�ͥ���������ȷ���Ƿ����ũ��ͱ�����',
                           '����̬�����ֲ����ֳ�ũ��ͱ�',
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
                           '�ȳԳ��еͱ��ֳ�ũ��ͱ�',
                           '���еͱ���ũ��ͱ�ֻ������һ��',
                           '�Գ��еͱ��ֳ�ũ��ͱ�',
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
                           '���еͱ����֤δ�鵽',
                           '�ȽϹ����˿���Ϣ��δ�鵽���еͱ������֤��Ϣ��ȷ����ͱ���Ա������Ƿ���ȷ',
                           '���еͱ����֤δ�鵽',
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
                           '���еͱ����֤������һ��',
                           '�ȽϹ����˿���Ϣ���������֤һ�¶�������һ�µĳ��еͱ��ˡ�ȷ����ͱ���Ա������Ƿ���ȷ',
                           '���еͱ����֤������һ��',
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
                           'ũ���屣���֤δ�鵽',
                           '�ȽϹ����˿���Ϣ��δ�鵽ũ���屣�����֤��Ϣ��ȷ����ũ���屣�˵�����Ƿ���ȷ',
                           'ũ���屣���֤δ�鵽',
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
                           'ũ���屣���֤������һ��',
                           '�ȽϹ����˿���Ϣ���������֤һ�¶�������һ�µ�ũ���屣�ˡ�ȷ����ũ���屣�˵�����Ƿ���ȷ',
                           'ũ���屣���֤������һ��',
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
                           '��ũҵ���������ֳԳ��еͱ�',
                           '����ũҵ���������ļ�ͥ���������ȷ���Ƿ���ϳ��еͱ�����',
                           '��ũҵ�����ֳԳ��еͱ�',
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
                           '���˸����ֲ��������ֳԳ��еͱ�',
                           '��ʵ���˸����ֲ�������ļ�ͥ���������ȷ���Ƿ���ϳ��еͱ�����',
                           '���˸����ֲ����ֳԳ��еͱ�',
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
                           '����̬�����ֲ��������ֳԳ��еͱ�',
                           '��ʵ����̬�����ֲ�������ļ�ͥ���������ȷ���Ƿ���ϳ��еͱ�����',
                           '����̬�����ֲ����ֳԳ��еͱ�',
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
                           '�쵼�ɲ������Գ��еͱ�',
                           '�˲��쵼�ɲ������Գ��еͱ������ȷ�����ͥ�����Ƿ�������߹涨',
                           '�쵼�ɲ������Գ��еͱ�',
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
                           '��ɲ������Գ��еͱ�',
                           '�˲��ɲ������Գ��еͱ������ȷ�����ͥ�����Ƿ�������߹涨',
                           '��ɲ������Գ��еͱ�',
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
                           '�������Գ��еͱ�',
                           '�˲�������Ա���ڳԳ��еͱ����',
                           '��������90�컹�Գ��еͱ�',
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
                           '��������ũ���屣',
                           '�˲�������Ա���ڳ�ũ���屣���',
                           '��������90�컹��ũ���屣',
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
                           '�˲�ֱϵ�����й����Գ��еͱ�',
                           '��ʵֱϵ�����й����Գ��еͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����й����Գ��еͱ�',
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
                           '�˲�ֱϵ�����й�˾�Գ��еͱ�',
                           '��ʵֱϵ�����й�˾�Գ��еͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����й�˾�Գ��еͱ�',
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
                           '�˲�ֱϵ�����г��Գ��еͱ�',
                           '��ʵֱϵ�����г��Գ��еͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����г��Գ��еͱ�',
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
                           '�˲�ֱϵ��������ũ���Գ��еͱ�',
                           '��ʵֱϵ��������ũ���Գ��еͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ��������ũ���Գ��еͱ�',
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
                           '�˲�ֱϵ�����з��Գ��еͱ�',
                           '��ʵֱϵ�����з��Գ��еͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����з��Գ��еͱ�',
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
                           '�ȳ�ũ��ͱ��ֳ�ũ���屣',
                           'ũ���屣��ũ��ͱ�ֻ������һ��',
                           '��ũ��ͱ��ֳ�ũ���屣',
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
                           '�ȳԳ��еͱ��ֳ�ũ���屣',
                           'ũ���屣�ͳ��еͱ�ֻ������һ��',
                           '�Գ��еͱ��ֳ�ũ���屣',
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
                           '��ũҵ���������ֳ�ũ��ͱ�',
                           '��ʵ��ũҵ��������ļ�ͥ���������ȷ���Ƿ����ũ��ͱ�����',
                           '��ũҵ�����ֳ�ũ��ͱ�',
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
                           '��ũҵ���������ֳ�ũ���屣',
                           '����ũҵ��������ļ�ͥ���������ȷ���Ƿ����ũ���屣����',
                           '��ũҵ�����ֳ�ũ���屣',
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
                           '���˸����ֲ��������ֳ�ũ���屣',
                           '�����˸����ֲ�������ļ�ͥ���������ȷ���Ƿ����ũ���屣����',
                           '���˸����ֲ����ֳ�ũ���屣',
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
                           '����̬�����ֲ��������ֳ�ũ���屣',
                           '������̬�����ֲ�������ļ�ͥ���������ȷ���Ƿ����ũ���屣����',
                           '����̬�����ֲ����ֳ�ũ���屣',
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
                           '�쵼�ɲ�������ũ���屣',
                           '�˲��쵼�ɲ�������ũ���屣�����ȷ�����ͥ�����Ƿ�������߹涨',
                           '�쵼�ɲ�������ũ���屣',
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
                           '��ɲ�������ũ���屣',
                           '�˲��ɲ�������ũ���屣�����ȷ�����ͥ�����Ƿ�������߹涨',
                           '��ɲ�������ũ���屣',
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
                           '�˲�ֱϵ�����й�����ũ���屣',
                           '��ʵֱϵ�����й�����ũ���屣�˼�ͥ���������ȷ���Ƿ�����屣����',
                           'ֱϵ�����й�����ũ���屣',
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
                           '�˲�ֱϵ�����й�˾��ũ���屣',
                           '��ʵֱϵ�����й�˾��ũ���屣�˼�ͥ���������ȷ���Ƿ�����屣����',
                           'ֱϵ�����й�˾��ũ���屣',
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
                           '�˲�ֱϵ�����г���ũ���屣',
                           '��ʵֱϵ�����г���ũ���屣�˼�ͥ���������ȷ���Ƿ�����屣����',
                           'ֱϵ�����г���ũ���屣',
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
                           '�˲�ֱϵ��������ũ����ũ���屣',
                           '��ʵֱϵ��������ũ����ũ���屣�˼�ͥ���������ȷ���Ƿ�����屣����',
                           'ֱϵ��������ũ����ũ���屣',
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
                           '�˲�ֱϵ�����з���ũ���屣',
                           '��ʵֱϵ�����з���ũ���屣�˼�ͥ���������ȷ���Ƿ�����屣����',
                           'ֱϵ�����з���ũ���屣',
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
                           '�ȳ�ũ���屣��ɢ�����ֳ�ũ�弯�й���',
                           'ͬʱֻ������һ�ֹ�����ʽ',
                           '��ũ���屣�ֳ�ũ���屣',
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
                           'ƶ�������֤δ�鵽',
                           '�ȽϹ����˿���Ϣ��δ�鵽ƶ���������֤��Ϣ��ȷ��ƶ������Ա������Ƿ���ȷ',
                           'ƶ�������֤δ�鵽',
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
                           'ƶ�������֤������һ��',
                           '�ȽϹ����˿���Ϣ���������֤һ�¶�������һ�µ�ƶ������ȷ��ƶ����������Ƿ���ȷ',
                           'ƶ�������֤������һ��',
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
                           '��ũҵ���������ƶ����',
                           '��ʵ��ũҵ��������ļ�ͥ���������ȷ���Ƿ����ƶ��������',
                           '��ũҵ����ũҵ������ƶ����ƶ����',
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
                           '���˸����ֲ��������ƶ����',
                           '��ʵ���˸����ֲ�������ļ�ͥ���������ȷ���Ƿ����ƶ��������',
                           '���˸�������ƶ����ƶ����',
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
                           '����̬�����ֲ��������ƶ����',
                           '��ʵ����̬�����ֲ�������ļ�ͥ���������ȷ���Ƿ����ƶ��������',
                           '����̬��������ƶ����ƶ����',
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
                           '����������Ա��ƶ����',
                           '�˲����������Ա��ͥ���������ȷ���Ƿ����ƶ������׼��',
                           '����������Աƶ����ƶ����',
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
                           '�쵼�ɲ���ƶ����',
                           '�˲��쵼�ɲ���ͥ���������ȷ���Ƿ����ƶ������׼��',
                           '�쵼�ɲ�ƶ����ƶ����',
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
                           '�쵼�ɲ�������ƶ����',
                           '�˲��쵼�ɲ��������������ȷ���Ƿ����ƶ������׼',
                           '�쵼����ƶ����ƶ����',
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
                           '��ɲ���ƶ����',
                           '�˲��ɲ����������ȷ���Ƿ����ƶ������׼��',
                           '��ɲ�ƶ����ƶ����',
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
                           '��ɲ�������ƶ����',
                           '�˲��ɲ������ľ��������ȷ���Ƿ����ƶ������׼',
                           '�����ƶ����ƶ����',
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
                           'ƶ�����й�������',
                           '��ʵƶ�����������������ȷ���Ƿ����ƶ������׼',
                           '�й���ƶ����ƶ����',
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
                           'ƶ����ֱϵ�����й�������',
                           '�˲�ƶ����ֱϵ�����������������ȷ���Ƿ����ƶ������׼',
                           'ֱϵ�����й���ƶ����ƶ����',
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
                           'ƶ�����й�˾',
                           '��ʵƶ�����й�˾�����ȷ���Ƿ���ϵͱ�����',
                           '�й�˾ƶ����ƶ����',
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
                           'ƶ����ֱϵ�����й�˾',
                           '��ʵֱϵ�����й�˾ƶ�������������ȷ���Ƿ����ƶ������׼',
                           'ֱϵ�����й�˾ƶ����ƶ����',
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
                           'ƶ�����г�',
                           '��ʵƶ������ͥ���������ȷ���Ƿ����ƶ������׼',
                           '�г�ƶ����ƶ����',
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
                           'ƶ����ֱϵ�����г�',
                           '��ʵƶ������ͥ���������ȷ���Ƿ����ƶ������׼',
                           'ֱϵ�����г�ƶ����ƶ����',
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
                           'ƶ�����������',
                           '�˲�ƶ�����������',
                           '��������90�컹ƶ����ƶ����',
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
                           'ƶ��������ũ�����',
                           '��ʵ����ũ��ƶ������ͥ���������ȷ���Ƿ����ƶ������׼',
                           '����ũ��ƶ����ƶ����',
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
                           'ƶ����ֱϵ��������ũ��',
                           '��ʵֱϵ��������ũ�������ȷ���Ƿ����ƶ������׼',
                           'ֱϵ��������ũ��ƶ����ƶ����',
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
                           'ƶ����������Ʒ��',
                           '��ʵ������Ʒ����ƶ������ͥ���������ȷ���Ƿ����ƶ������׼',
                           '�з�ƶ����ƶ����',
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
                           'ƶ����ֱϵ�����з�',
                           '��ʵֱϵ�����з�ƶ������ͥ���������ȷ���Ƿ����ƶ������׼',
                           'ֱϵ�����з�ƶ����ƶ����',
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
                           '��ƶ���������׵ذ�Ǩ����',
                           '�˲��ƶ���������׵ذ�Ǩ����',
                           'ƶ�����׵ط�ƶ��Ǩ�׵ط�ƶ��Ǩ',
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
                           '�ظ������׵ط�ƶ��Ǩ',
                           '�ظ������׵ط�ƶ��Ǩ',
                           '����������Ա�׵ط�ƶ��Ǩ�׵ط�ƶ��Ǩ',
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
                           '������ط�ƶ��Ǩ����Σ�������ʽ�',
                           '�˲��������ط�ƶ��Ǩ����Σ�������ʽ�',
                           '���׵ط�ƶ��Ǩ�׵ط�ƶ��Ǩ',
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
                           '�׵������Ǩ�˾������׼�쳣',
                           '�˲鲻����׼���׵������Ǩ',
                           '������׵ط�ƶ��Ǩ�׵ط�ƶ��Ǩ',
                           52,
                           45,
                           0,
                           20,
                           ' AND ((ƽ����� > 25 * 1.1 AND ��ͥ�˿� > 1) OR (ƽ����� > 40 AND ��ͥ�˿� = 1) OR (��� > 125 AND    ��ͥ�˿� > 5)) ',
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
                           '�������첹�����֤δ�鵽',
                           '�˲�������첹���Ƿ���ʵ',
                           '����������Ѳ������֤δ�鵽',
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
                           '�������첹���������֤��һ��',
                           '�˲�������첹���Ƿ���ʵ',
                           '����������Ѳ������֤������һ��',
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
                           '����޲���δ���뽨������ƶ����',
                           '�˲�����޲���ѧ���Ƿ���ƶ����',
                           'ƶ���������������Ѳ���',
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
                           '����������������쳣',
                           '�˲������쳣�����������',
                           '�޵ͱ��屣�����������Ѳ���',
                           53,
                           65,
                           0,
                           21,
                           '(���� >16 or ���� < 5)',
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
                           '����ͨ���в��������쳣',
                           '�˲���ͨ�����첹���������쳣�����',
                           '�޵ͱ��屣�������ѧ��',
                           54,
                           65,
                           0,
                           21,
                           '(���� >19 or ���� < 15)',
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
                           '����ͨ������ѧ��δ���뽨������ƶ����',
                           '�˲�����ͨ������ѧ��ѧ���Ƿ���ƶ����',
                           'ƶ�����������ѧ��',
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
                           '����ͨ������ѧ�����֤δ�鵽',
                           '�˲�����ͨ������ѧ���Ƿ���ʵ',
                           '������ѧ�����֤δ�鵽',
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
                           '����ͨ������ѧ���������֤��һ��',
                           '�˲�����ͨ������ѧ���Ƿ���ʵ',
                           '������ѧ�����֤������һ��',
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
                           '������¶�ƻ�ѧ��δ���뽨������ƶ����',
                           '������¶�ƻ�ѧ���Ƿ���ƶ����',
                           'ƶ��������¶�ƻ�',
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
                           '������¶�ƻ�ѧ�����֤δ�鵽',
                           '�˲�������¶�ƻ�ѧ���Ƿ���ʵ',
                           '��¶�ƻ����֤δ�鵽',
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
                           '������¶�ƻ�ѧ���������֤��һ��',
                           '�˲���¶�ƻ�ѧ���Ƿ���ʵ',
                           '��¶�ƻ����֤������һ��',
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
                           '����ְ��ѧ�����֤δ�鵽',
                           '�˲�����ְ��ѧ���Ƿ���ʵ',
                           '��ְ��ѧ�����֤δ�鵽',
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
                           '����ְ��ѧ���������֤��һ��',
                           '�˲�����ְ��ѧ���Ƿ���ʵ',
                           '��ְ��ѧ�����֤������һ��',
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
                           '����ְ��ѧ��ѧ�������쳣',
                           '�˲���ְ��ѧ��ѧ�������쳣�����',
                           '���������ְ��ѧ��',
                           55,
                           45,
                           0,
                           21,
                           '(���� >19 or ���� < 14)',
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
                           '���ܷ�ƶ���������֤δ�鵽',
                           '�˲����ܷ�ƶ�������Ƿ���ʵ',
                           '��ƶ�Ŵ����֤δ�鵽',
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
                           '�����ܷ�ƶ�������������֤��һ��',
                           '�˲����ܷ�ƶ�������Ƿ���ʵ',
                           '��ƶ�Ŵ����֤������һ��',
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
                           '�����ƶ�Ŵ�δ���뽨������ƶ����',
                           '�����ƶ�Ŵ��Ƿ���ƶ����',
                           'ƶ�������ƶ�Ŵ�',
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
                           '�˲��ƶ�Ŵ�ʹ�����',
                           '�˲��ƶ�Ŵ�ʹ��������Ƿ����ڷ�չ',
                           '����������Ա���ƶ�Ŵ�',
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
                           '�׵ط�ƶ��Ǩ���֤δ�鵽',
                           '�˲��׵ط�ƶ��Ǩ�Ƿ���ʵ',
                           '�׵ط�ƶ��Ǩ���֤δ�鵽',
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
                           '�׵ط�ƶ��Ǩ�������֤��һ��',
                           '�˲��׵ط�ƶ��Ǩ�Ƿ���ʵ',
                           '�׵ط�ƶ��Ǩ���֤������һ��',
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
                           '��������������������������֤��һ��',
                           '�˲��������������������Ƿ���ʵ',
                           '�������������������֤������һ��',
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
                           '����������������������֤δ�鵽',
                           '�˲��������������������Ƿ���ʵ',
                           '�������������������֤δ�鵽',
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
                           '�˲�סԺ�ڼ�������',
                           '�˲�סԺ�ڼ�������',
                           'סԺ�����������������',
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
                           '��ƶ�����ʽ����֤δ�鵽',
                           '�ȽϹ����˿���Ϣ���˲��ƶ�����ʽ������֤��Ϣ��',
                           '��ƶ�������֤δ�鵽',
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
                           '���ƶ�����ʽ����֤������һ��',
                           '�ȽϹ����˿���Ϣ���������֤һ�¶�������һ�µķ�ƶ�����ʽ�',
                           '��ƶ�������֤������һ��',
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
                           '��ƶ�������ƶ�����ʽ�',
                           '�˲��ƶ�������ƶ�����ʽ�',
                           'ƶ������ƶ�����ʽ��ƶ����',
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
                           'ƶ����δ�쵽��ƶ�����ʽ�',
                           '�˲�ƶ����δ�쵽��ƶ�����ʽ�',
                           '��ƶ�����ʽ��ƶ������ƶ����ƶ����',
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
                           'ƶ����δ�컧��ͨ',
                           '�˲�ƶ����δ�컧��ͨ',
                           '���ܻ���ͨ��ƶ����ƶ����',
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
                           '��ƶ�����컧��ͨ',
                           '�˲��ƶ�����컧��ͨ',
                           'ƶ�������ܻ���ͨ',
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
                           '����ͨ���֤δ�鵽',
                           '�ȽϹ����˿���Ϣ���˲黧��ͨ���֤��Ϣ��',
                           '����ͨ���֤δ�鵽',
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
                           '�컧��ͨ���֤������һ��',
                           '�ȽϹ����˿���Ϣ���������֤һ�¶�������һ�µ��컧��ͨƶ������',
                           '����ͨ���֤������һ��',
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
                           '�˲�Ȳμ���ũ���ֲμ�ҽ��(����ְ��)�α����',
                           '�˲�Ȳμ���ũ���ֲμ�ҽ��(����ְ��)�α����',
                           'ҽ������ũ��',
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
                           '�˲���ũ��ˢ������',
                           '�˲���ũ��ˢ������',
                           '���������ũ��',
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
                           'ְҵ���ܲ����������֤δ�鵽',
                           '�˲�ְҵ���ܲ��������Ƿ���ʵ',
                           'ְҵ�������֤δ�鵽',
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
                           'ְҵ���ܲ��������������֤��һ��',
                           '�˲�ְҵ���ܲ��������Ƿ���ʵ',
                           'ְҵ�������֤������һ��',
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
                           'ְҵ��ѵ�����������֤δ�鵽',
                           '�˲�ְҵ��ѵ���������Ƿ���ʵ',
                           'ְҵ��ѵ���֤δ�鵽',
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
                           'ְҵ��ѵ���������������֤��һ��',
                           '�˲�ְҵ��ѵ���������Ƿ���ʵ',
                           'ְҵ��ѵ���֤������һ��',
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
                           '��ҵ��ѵ�����������֤δ�鵽',
                           '�˲鴴ҵ��ѵ���������Ƿ���ʵ',
                           '��ҵ��ѵ���֤δ�鵽',
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
                           '��ҵ��ѵ���������������֤��һ��',
                           '�˲鴴ҵ��ѵ���������Ƿ���ʵ',
                           '��ҵ��ѵ���֤������һ��',
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
                           '���Ѳм������֤δ�鵽',
                           '�˲����Ѳм����Ƿ���ʵ',
                           '���Ѳм��������֤δ�鵽',
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
                           '���Ѳм����������֤��һ��',
                           '�˲����Ѳм����Ƿ���ʵ',
                           '���Ѳм��������֤������һ��',
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
                           '�ضȲм������֤δ�鵽',
                           '�˲��ضȲм����Ƿ���ʵ',
                           '�ضȲм��˻��������֤δ�鵽',
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
                           '�ضȲм����������֤��һ��',
                           '�˲��ضȲм����Ƿ���ʵ',
                           '�ضȲм��˻��������֤������һ��',
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
                           '�˲��촴ҵ��ѵֻ����һ��',
                           '��ҵ��ѵֻ����һ��',
                           '������촴ҵ��ѵ',
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
                           '�˲�ְҵ���������һ����һ�Σ�',
                           'һ��һ��',
                           '����������Ա��ְҵ����',
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
                           'ƶ���������ϱ���',
                           '�˲�ƶ���������ϱ������',
                           '�����ϱ���ƶ����ƶ����',
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
                           '��ҵ��ѵ��δ���칫˾',
                           '�˲鴴ҵ���Ƿ��칤��ִ�ա�',
                           '�ܽ���촴ҵ��ѵ',
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
                           '�˲�ְҵ���ܲ�����ȡ�����һ����һ�Σ�',
                           'һ��һ��',
                           '�ܽ����ְҵ��ѵ',
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
                           '���Ѳм���δ���뽨������ƶ����',
                           '�˲����Ѳм���δ����ƶ�������',
                           'ƶ���������Ѳм�����',
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
                           '�˲��б������Ĺ�˾ǰ20��',
                           '�˲��б������Ĺ�˾',
                           '����������ƽ������ƽ�',
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
                           '�˲��б������Ĺ�˾ǰ5��(������)',
                           '�˲��б������Ĺ�˾(������)',
                           '�ܽ������ƽ�����ƽ�',
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
                           '��С����Ա��ƶ����',
                           '�˲���С����Ա���������ȷ���Ƿ����ƶ������׼��',
                           '��ƶ����ƶ����',
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
                           '��С����Ա������ƶ����',
                           '�˲���С���������������ȷ���Ƿ����ƶ������׼',
                           '��С����Ա������ƶ����ƶ����',
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
                           '��С����Ա��ũ��ͱ�',
                           '�˲���С����Ա�Ƿ���ũ��ͱ�����',
                           '��С����Ա���ũ��ͱ�',
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
                           '�˲���С����Ա������ũ��ͱ�',
                           '�˲���С����Ա������ũ��ͱ������ȷ�����ͥ�����Ƿ�������߹涨',
                           '��С����Ա����С����Ա�������ũ��ͱ�',
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
                           '��С����Ա�Գ��еͱ�',
                           '�˲���С����Ա�ǲ��ǳ��еͱ�����',
                           '��С����Ա��Գ��еͱ�',
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
                           '��С����Ա�����Գ��еͱ�',
                           '�˲���С����Ա�����Գ��еͱ������ȷ�����ͥ�����Ƿ�������߹涨',
                           '��С����Ա����С����Ա������Գ��еͱ�',
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
                           '��С����Ա��ũ���屣',
                           '�˲���С����Ա�ǲ���ũ���屣����',
                           '��С����Ա���ũ���屣',
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
                           '��С����Ա������ũ���屣',
                           '�˲���С����Ա������ũ���屣�����ȷ�����ͥ�����Ƿ�������߹涨',
                           '��С����Ա����С����Ա�������ũ���屣',
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
                           'ƶ�����й��̵Ǽ�',
                           '��ʵƶ�����й��̵Ǽǣ�ȷ���Ƿ���ϵͱ�����',
                           '�й��̵Ǽ�ƶ����ƶ����',
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
                           'ƶ����ֱϵ�����й��̵Ǽ�',
                           '��ʵƶ����ֱϵ�����й��̵Ǽǵ������ȷ���Ƿ����ƶ������׼',
                           'ֱϵ�����й��̵Ǽ�ƶ����ƶ����',
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
                           '�й��̵Ǽǳ�ũ��ͱ�',
                           '��ʵ�й��̵Ǽǳ�ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�й��̵Ǽǳ�ũ��ͱ�',
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
                           '�˲�ֱϵ�����й��̵Ǽǳ�ũ��ͱ�',
                           '��ʵֱϵ�����й��̵Ǽǳ�ũ��ͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����й��̵Ǽǳ�ũ��ͱ�',
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
                           '�й��̵ǼǳԳ��еͱ�',
                           '��ʵ�й��̵ǼǳԳ��еͱ���Ա��ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�й��̵ǼǳԳ��еͱ�',
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
                           '�˲�ֱϵ�����й��̵ǼǳԳ��еͱ����',
                           '��ʵֱϵ�����й��̵ǼǳԳ��еͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����й��̵ǼǳԳ��еͱ�',
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
                           '�й��̵Ǽǳ�ũ���屣',
                           '��ʵ�й��̵Ǽǳ�ũ���屣��Ա��ͥ���������ȷ���Ƿ����ũ���屣����',
                           '�й��̵Ǽǳ�ũ���屣',
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
                           '�˲�ֱϵ�����й��̵Ǽǳ�ũ���屣',
                           '��ʵֱϵ�����й��̵Ǽǳ�ũ���屣�˼�ͥ���������ȷ���Ƿ�����屣����',
                           'ֱϵ�����й��̵Ǽǳ�ũ���屣',
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
                           '�з��Գ��еͱ�',
                           '��ʵ�з��Գ��еͱ���Ա��ͥ���������ȷ���Ƿ���ϵͱ�����',
                           '�з��Գ��еͱ�',
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
                           '�˲��ͥ��2�׷��Գ��еͱ�',
                           '��ʵ��ͥ��2�׷��Գ��еͱ��˼�ͥ���������ȷ���Ƿ���ϵͱ�����',
                           'ֱϵ�����з��Գ��еͱ�',
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
                           '�˲�ְҵ��ѵʱ���쳣���',
                           '�˲�ְҵ��ѵʱ���쳣���',
                           '�������ְҵ��ѵ',
                           61,
                           45,
                           0,
                           30,
                           '',
                           1,
                           720030
                       );


-- ��DataCheckRules
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
                               'ɾ������������Ա���쵼�ɲ�����Ϣ',
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
                               'ɾ������������Ա�д�ɲ�����Ϣ',
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
                               '�쵼�ɲ������ϢУ��',
                               'SELECT DISTINCT '''' AS ���,
                ''�쵼�ɲ����֤δ�鵽'' AS ����,
                tbCompareLeaderInfo.Addr AS ��λ,
                tbCompareLeaderInfo.ID AS ���֤��,
                tbCompareLeaderInfo.Name AS ����,
                '''' AS ��������
  FROM tbCompareLeaderInfo
 WHERE tbCompareLeaderInfo.ID NOT IN
       (SELECT tbComparePersonInfo.ID
          FROM tbComparePersonInfo) 
UNION ALL
SELECT DISTINCT '''' AS ���,
                ''�쵼�ɲ����֤������һ��'' AS ����,
                tbCompareLeaderInfo.Addr AS ��λ,
                tbCompareLeaderInfo.ID AS ���֤��,
                tbCompareLeaderInfo.Name AS ����,
                tbComparePersonInfo.Name AS ��������
  FROM tbCompareLeaderInfo
       JOIN tbComparePersonInfo ON tbCompareLeaderInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareLeaderInfo.Name AND 
       (length(tbCompareLeaderInfo.ID) = 15 OR 
        length(tbCompareLeaderInfo.ID) = 18) 
 ORDER BY ��λ,
          ���֤��',
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
                               '�쵼�ɲ����������ϢУ��',
                               'SELECT DISTINCT '''' AS ���,''�쵼�ɲ��������֤δ�鵽'' AS ����,
                tbCompareLeaderRelateInfo.Addr AS ��λ,
                tbCompareLeaderRelateInfo.ID AS ���֤��,
                tbCompareLeaderRelateInfo.Name AS ����,
                '''' AS ��������
  FROM tbCompareLeaderRelateInfo
 WHERE tbCompareLeaderRelateInfo.ID NOT IN
       (SELECT tbComparePersonInfo.ID
          FROM tbComparePersonInfo) 
UNION ALL
SELECT DISTINCT '''' AS ���,''�쵼�ɲ��������֤������һ��'' AS ����,
                tbCompareLeaderRelateInfo.Addr AS ��λ,
                tbCompareLeaderRelateInfo.ID AS ���֤��,
                tbCompareLeaderRelateInfo.Name AS ����,
                tbComparePersonInfo.Name AS ��������
  FROM tbCompareLeaderRelateInfo
       JOIN tbComparePersonInfo ON tbCompareLeaderRelateInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareLeaderRelateInfo.Name AND 
       (length(tbCompareLeaderRelateInfo.ID) = 15 OR 
        length(tbCompareLeaderRelateInfo.ID) = 18) 
 ORDER BY ��λ,
          ���֤��',
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
                               '��ɲ������ϢУ��',
                               'SELECT DISTINCT '''' AS ���,
                ''��ɲ����֤δ�鵽'' AS ����,
                tbComparecountryInfo.Region AS ����ֵ�,
                tbComparecountryInfo.Addr AS ��λ,
                tbComparecountryInfo.ID AS ���֤��,
                tbComparecountryInfo.Name AS ����,
                '''' AS ��������
  FROM tbComparecountryInfo
 WHERE tbComparecountryInfo.ID NOT IN (
           SELECT tbComparePersonInfo.ID
             FROM tbComparePersonInfo
       )
UNION ALL
SELECT DISTINCT '''' AS ���,
                ''��ɲ����֤������һ��'' AS ����,
                tbComparecountryInfo.Region AS ����ֵ�,
                tbComparecountryInfo.Addr AS ��λ,
                tbComparecountryInfo.ID AS ���֤��,
                tbComparecountryInfo.Name AS ����,
                tbComparePersonInfo.Name AS ��������
  FROM tbComparecountryInfo
       JOIN
       tbComparePersonInfo ON tbComparecountryInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbComparecountryInfo.Name AND 
       (length(tbComparecountryInfo.ID) = 15 OR 
        length(tbComparecountryInfo.ID) = 18) 
 ORDER BY ��λ,
          ���֤��;
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
                               '��ɲ����������ϢУ��',
                               'SELECT DISTINCT '''' AS ���,
                ''��ɲ��������֤δ�鵽'' AS ����,
                tbComparecountryRelateInfo.Region AS ����ֵ�,
                tbComparecountryRelateInfo.RelateName AS ��ɲ�����,
                tbComparecountryRelateInfo.Addr AS ��λ,
                tbComparecountryRelateInfo.ID AS ���֤��,
                tbComparecountryRelateInfo.Name AS ����,
                '''' AS ��������
  FROM tbComparecountryRelateInfo
 WHERE tbComparecountryRelateInfo.ID NOT IN (
           SELECT tbComparePersonInfo.ID
             FROM tbComparePersonInfo
       )
UNION ALL
SELECT DISTINCT '''' AS ���,
                ''��ɲ��������֤������һ��'' AS ����,
                tbComparecountryRelateInfo.Region AS ����ֵ�,
                tbComparecountryRelateInfo.RelateName AS ��ɲ�����,
                tbComparecountryRelateInfo.Addr AS ��λ,
                tbComparecountryRelateInfo.ID AS ���֤��,
                tbComparecountryRelateInfo.Name AS ����,
                tbComparePersonInfo.Name AS ��������
  FROM tbComparecountryRelateInfo
       JOIN
       tbComparePersonInfo ON tbComparecountryRelateInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbComparecountryRelateInfo.Name AND 
       (length(tbComparecountryRelateInfo.ID) = 15 OR 
        length(tbComparecountryRelateInfo.ID) = 18) 
 ORDER BY ��λ,
          ���֤��;
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
                               '�޼�����Ϣ����С����Ա',
                               'SELECT DISTINCT '''' AS ���,
                region AS ����ֵ�,
                Addr AS ��λ,
                ID AS ���֤��,
                Name AS ����
  FROM tbCompareTownFive
 WHERE id NOT IN (
           SELECT b.RelateID
             FROM tbCompareTownFive a,
                  tbCompareTownFiveRelate b
            WHERE a.ID == b.RelateID
       )
 ORDER BY ����ֵ�',
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
                               '����������Ա�����ϢУ��',
                               'SELECT DISTINCT '''' AS ���,
                ''����������Ա���֤δ�鵽'' AS ����,
                tbCompareCivilInfo.Addr AS ��λ,
                tbCompareCivilInfo.ID AS ���֤��,
                tbCompareCivilInfo.Name AS ����,
                '''' AS ��������
  FROM tbCompareCivilInfo
 WHERE tbCompareCivilInfo.ID NOT IN
       (SELECT tbComparePersonInfo.ID
          FROM tbComparePersonInfo) 
UNION ALL
SELECT DISTINCT '''' AS ���,
                ''����������Ա���֤������һ��'' AS ����,
                tbCompareCivilInfo.Addr AS ��λ,
                tbCompareCivilInfo.ID AS ���֤��,
                tbCompareCivilInfo.Name AS ����,
                tbComparePersonInfo.Name AS ��������
  FROM tbCompareCivilInfo
       JOIN tbComparePersonInfo ON tbCompareCivilInfo.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareCivilInfo.Name AND 
       (length(tbCompareCivilInfo.ID) = 15 OR 
        length(tbCompareCivilInfo.ID) = 18) 
 ORDER BY ��λ,
          ���֤��',
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
                               '�޼�����Ϣ�쵼',
                               'SELECT DISTINCT '''' AS ���,
                Addr AS ��λ,
                ID AS ���֤��,
                Name AS ����
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
                               '�޼�����Ϣ��ɲ�',
                               'SELECT DISTINCT '''' AS ���,
                region ����ֵ�,
                Addr AS ��λ,
                ID AS ���֤��,
                Name AS ����
  FROM tbComparecountryInfo
 WHERE id NOT IN (
           SELECT a.ID
             FROM tbComparecountryInfo a,
                  tbComparecountryRelateInfo b
            WHERE a.ID == b.RelateID
       )
 ORDER BY ����ֵ�',
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
                               '��С����Ա��ϢУ��',
                               'SELECT DISTINCT '''' AS ���,
                ''��С����Ա���֤δ�鵽'' AS ����,
                tbCompareTownFive.Addr AS ��λ,
                tbCompareTownFive.ID AS ���֤��,
                tbCompareTownFive.Name AS ����,
                '''' AS ��������
  FROM tbCompareTownFive
 WHERE tbCompareTownFive.ID NOT IN (
           SELECT tbComparePersonInfo.ID
             FROM tbComparePersonInfo
       )
UNION ALL
SELECT DISTINCT '''' AS ���,
                ''��С����Ա���֤������һ��'' AS ����,
                tbCompareTownFive.Addr AS ��λ,
                tbCompareTownFive.ID AS ���֤��,
                tbCompareTownFive.Name AS ����,
                tbComparePersonInfo.Name AS ��������
  FROM tbCompareTownFive
       JOIN
       tbComparePersonInfo ON tbCompareTownFive.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareTownFive.Name AND 
       (length(tbCompareTownFive.ID) = 15 OR 
        length(tbCompareTownFive.ID) = 18) 
 ORDER BY ��λ,
          ���֤��',
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
                               '��С����Ա������ϢУ��',
                               'SELECT DISTINCT '''' AS ���,
                ''�쵼�ɲ��������֤δ�鵽'' AS ����,
                tbCompareTownFiveRelate.Addr AS ��λ,
                tbCompareTownFiveRelate.ID AS ���֤��,
                tbCompareTownFiveRelate.Name AS ����,
                '''' AS ��������
  FROM tbCompareTownFiveRelate
 WHERE tbCompareTownFiveRelate.ID NOT IN (
           SELECT tbComparePersonInfo.ID
             FROM tbComparePersonInfo
       )
UNION ALL
SELECT DISTINCT '''' AS ���,
                ''�쵼�ɲ��������֤������һ��'' AS ����,
                tbCompareTownFiveRelate.Addr AS ��λ,
                tbCompareTownFiveRelate.ID AS ���֤��,
                tbCompareTownFiveRelate.Name AS ����,
                tbComparePersonInfo.Name AS ��������
  FROM tbCompareTownFiveRelate
       JOIN
       tbComparePersonInfo ON tbCompareTownFiveRelate.ID = tbComparePersonInfo.ID
 WHERE tbComparePersonInfo.Name <> tbCompareTownFiveRelate.Name AND 
       (length(tbCompareTownFiveRelate.ID) = 15 OR 
        length(tbCompareTownFiveRelate.ID) = 18) 
 ORDER BY ��λ,
          ���֤��',
                               1,
                               1,
                               99999999999
                           );


-- ��CompareAimTest
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


-- ��DataFormat
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '��ַ',
                           '��ַ',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '�ͺ�',
                           '�ͺ�',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����',
                           '����',
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
                           '��ַ',
                           '��ַ',
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
                           '���',
                           '���',
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
                           '���',
                           '���',
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
                           '�����к�',
                           '�����к�',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '�ͺ�',
                           '�ͺ�',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '��ַ',
                           '��λ',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '��ַ',
                           '��λ',
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
                           '�ͺ�',
                           '����ְ��',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '����',
                           '����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����',
                           '����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��λ',
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
                           '���',
                           'ְ��',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '��ַ',
                           '��λ',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           'ũ��ͱ�����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '��������',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '�屣����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '��������',
                           '��������',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '���еͱ�����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '���',
                           '���',
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
                           '����',
                           '����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '��������',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����',
                           '����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '�α������֤��',
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
                           '����',
                           '�α�������',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ͥ��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '����',
                           '����',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '��Ժʱ��',
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
                           '����1',
                           '��Ժʱ��',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '�������֤��',
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
                           '����',
                           '��������',
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
                           '����',
                           '����',
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
                           '�ͺ�',
                           '��������',
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
                           '���',
                           '���ʱ��',
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
                           '���1',
                           '��֯��������',
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
                           '���2',
                           'Ӫҵ����',
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
                           '��ϵ',
                           'ͳһ������ñ���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '�������֤��',
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
                           '����',
                           '��������',
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
                           '��ַ',
                           'Ӫҵ��ַ',
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
                           '���������',
                           '��ҵ����',
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
                           '���2',
                           'ע���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '����',
                           '����',
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
                           '���֤��',
                           '���֤��',
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
                           '��ַ',
                           '��ַ',
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
                           '��ϵ',
                           '��ϵ',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '��Ǩ�����֤��',
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
                           '����',
                           '��Ǩ������',
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
                           '����ֵ�',
                           '��Ǩ������ֵ�',
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
                           '��ַ',
                           '��Ǩ����ַ',
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
                           '����',
                           '��Ǩ����',
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
                           '���',
                           '��ͥ����',
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
                           '��������',
                           '��װ��ʽ',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '�ͺ�',
                           '�ͺ�',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '���',
                           '���',
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
                           '����',
                           '����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '����',
                           '����',
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
                           '��ַ',
                           '��ַ',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '�������֤��',
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
                           '����',
                           '��������',
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
                           '��ַ',
                           '�쵼��λ',
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
                           '��������֤��',
                           '�쵼���֤��',
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
                           '���������',
                           '�쵼����',
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
                           '��ϵ',
                           '���쵼��ϵ',
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
                           '���',
                           'ְ��',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '�α�������',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '�α��˵�ַ',
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
                           '����',
                           'ˢ������',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '���������',
                           '���������',
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
                           '����',
                           '����',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '��ַ',
                           '��ַ',
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
                           '���',
                           '���',
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
                           '����',
                           '����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '��ַ',
                           '��ַ',
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
                           '���',
                           '���',
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
                           '����',
                           '����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��λ',
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
                           '���',
                           'ְ��',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '��������֤��',
                           '��������֤��',
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
                           '���������',
                           '���������',
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
                           '��ϵ',
                           '��ϵ',
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
                           '���',
                           '���',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '�������֤��',
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
                           '����',
                           '��������',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��������֤��',
                           '��ɲ����֤��',
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
                           '���������',
                           '��ɲ�����',
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
                           '��ϵ',
                           '���ɲ���ϵ',
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
                           '���',
                           'ְ��',
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
                           '��ַ',
                           '��λ',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '�������֤��',
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
                           '����',
                           '��������',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '���������',
                           '��˾����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           'ˢ������',
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
                           '���',
                           'ˢ�����',
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
                           '���������',
                           'ҽԺ',
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
                           '���',
                           '���ձ��',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '��������֤��',
                           '�������֤��',
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
                           '���������',
                           '��������',
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
                           '��ϵ',
                           '��ϵ',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '����',
                           '��Ŀ����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '�б�����',
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
                           '��������',
                           '��Ŀ����',
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
                           '���������',
                           '�б굥λ',
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
                           '��ϵ',
                           'ͳһ������ñ���',
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
                           '�ͺ�',
                           '����Ҫ��',
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
                           '���',
                           '��Ŀ���',
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
                           '���1',
                           '��֯��������',
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
                           '���2',
                           'Ӫҵִ��ע����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '���������',
                           'ѧУ����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '���������',
                           'ѧУ����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '����������֤��',
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
                           '����',
                           '�����������',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��������ַ',
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
                           '����',
                           '����ʱ��',
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
                           '��������֤��',
                           'ҽ�����֤��',
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
                           '���������',
                           'ҽ������',
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
                           '�ͺ�',
                           '����',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '�ͺ�',
                           '�ͺ�',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '�ͺ�',
                           'ְҵ���ܹ�˾',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '��ʼ����',
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
                           '���',
                           '���',
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
                           '�ͺ�',
                           '��ѵ��˾',
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
                           '����1',
                           '��������',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '����',
                           '����',
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
                           '���',
                           '���',
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
                           '����1',
                           '����1',
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
                           '�ͺ�',
                           '��ѵ��˾',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '����ֵ�',
                           '����ֵ�',
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
                           '��ַ',
                           '��ַ',
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
                           '��ʼ�к�',
                           '��ʼ�к�',
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
                           '���֤��',
                           '���֤��',
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
                           '����',
                           '����',
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
                           '�ͺ�',
                           '�ͺ�',
                           3,
                           NULL,
                           11
                       );


-- ��DataItem
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
                         '�ʽ�ȶ�',
                         '�������߼ල���',
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
                         'Դ����',
                         'Դ����',
                         0,
                         '\01Դ����',
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
                         '�ȶ�����',
                         '�ȶ�����',
                         0,
                         '\02�ȶ�����',
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
                         'ũ���屣',
                         '11ũ���屣���ʽ�',
                         0,
                         '\01Դ����\11ũ���屣���ʽ�',
                         '0001-01-01 00:00:00',
                         'tbSourceFiveGuaranteFamily',
                         1,
                         110,
                         '��',
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
                         'ũ��Σ��',
                         '22ũ��Σ�������ʽ�',
                         0,
                         '\02�ȶ�����\22ũ��Σ�������ʽ�',
                         '0001-01-01 00:00:00',
                         'tbSourceRuralBadHouse',
                         1,
                         10220,
                         '��',
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
                         'ũҵ����',
                         '17ũҵ֧�ֱ��������ʽ�',
                         0,
                         '\02�ȶ�����\17ũҵ֧�ֱ��������ʽ�',
                         '0001-01-01 00:00:00',
                         'tbSourceFoodAid',
                         1,
                         10170,
                         '��ũҵ����',
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
                         '����������Ա',
                         '01����������Ա����',
                         0,
                         '\02�ȶ�����\01����������Ա����',
                         '0001-01-01 00:00:00',
                         'tbCompareCivilInfo',
                         1,
                         10010,
                         '����������Ա',
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
                         '�쵼�ɲ�',
                         '02�쵼�ɲ�����',
                         0,
                         '\02�ȶ�����\02�쵼�ɲ�����',
                         '0001-01-01 00:00:00',
                         'tbCompareLeaderInfo',
                         1,
                         10020,
                         '�쵼�ɲ�',
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
                         '�쵼�ɲ�����',
                         '03�쵼�ɲ���������',
                         0,
                         '\02�ȶ�����\03�쵼�ɲ���������',
                         '0001-01-01 00:00:00',
                         'tbCompareLeaderRelateInfo',
                         1,
                         10030,
                         '�쵼����',
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
                         '��ɲ�',
                         '04��ɲ�����',
                         0,
                         '\02�ȶ�����\04��ɲ�����',
                         '0001-01-01 00:00:00',
                         'tbComparecountryInfo',
                         1,
                         10040,
                         '��ɲ�',
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
                         '��ɲ�����',
                         '05��ɲ���������',
                         0,
                         '\02�ȶ�����\05��ɲ���������',
                         '0001-01-01 00:00:00',
                         'tbComparecountryRelateInfo',
                         1,
                         10050,
                         '�����',
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
                         '��˰����',
                         '06������˰����',
                         0,
                         '\02�ȶ�����\06������˰����',
                         '0001-01-01 00:00:00',
                         'tbCompareIncomeTaxInfo',
                         1,
                         10060,
                         '�й���',
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
                         '��˾����',
                         '07��˾��˰����',
                         0,
                         '\02�ȶ�����\07��˾��˰����',
                         '0001-01-01 00:00:00',
                         'tbCompareCompanyInfo',
                         1,
                         10070,
                         '�й�˾',
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
                         '�˿�����',
                         '08�˿�����',
                         0,
                         '\02�ȶ�����\08�˿�����',
                         '0001-01-01 00:00:00',
                         'tbComparePersonInfo',
                         1,
                         10080,
                         '���֤������һ��',
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
                         '��������',
                         '09��������',
                         0,
                         '\02�ȶ�����\09��������',
                         '0001-01-01 00:00:00',
                         'tbCompareFamilyInfo',
                         1,
                         10090,
                         'ֱϵ����',
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
                         '�����Ǽ�',
                         '10����������',
                         0,
                         '\02�ȶ�����\10�����Ǽ�����',
                         '0001-01-01 00:00:00',
                         'tbCompareCarInfo',
                         1,
                         10100,
                         '�г�',
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
                         '��������',
                         '11������Ա����',
                         0,
                         '\02�ȶ�����\11������Ա����',
                         '0001-01-01 00:00:00',
                         'tbCompareDeathInfo',
                         1,
                         10110,
                         '��������',
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
                         '����ũ��',
                         '12ũ����������',
                         0,
                         '\02�ȶ�����\12ũ����������',
                         '0001-01-01 00:00:00',
                         'tbComparemachineInfo',
                         1,
                         10120,
                         '����ũ��',
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
                         '�𻯵Ǽ�',
                         '13�𻯵Ǽ�����',
                         0,
                         '\02�ȶ�����\13�𻯵Ǽ�',
                         '0001-01-01 00:00:00',
                         'tbCompareBurnInfo',
                         1,
                         10130,
                         '��������',
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
                         '��������',
                         '14�����Ǽ�����',
                         0,
                         '\02�ȶ�����\14��������',
                         '0001-01-01 00:00:00',
                         'tbCompareHouseInfo',
                         1,
                         10140,
                         '�з�',
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
                         '��̬������',
                         '19��̬�����ֲ����ʽ�',
                         0,
                         '\02�ȶ�����\19��̬�����ֲ����ʽ�',
                         '0001-01-01 00:00:00',
                         'tbSourceForestEnvAid',
                         1,
                         10190,
                         '��',
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
                         'ũ��ͱ�',
                         '09ũ��������������ʽ�',
                         0,
                         '\01Դ����\09ũ��������������ʽ�',
                         '0001-01-01 00:00:00',
                         'tbSourceSafeLowLifeContry',
                         1,
                         90,
                         '��',
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
                         '���еͱ�',
                         '10���о������������ʽ�',
                         0,
                         '\01Դ����\10���о������������ʽ�',
                         '0001-01-01 00:00:00',
                         'tbSourceSafeLowLifeCity',
                         1,
                         100,
                         '��',
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
                         '�˸�����',
                         '18�˸����ֲ����ʽ�',
                         0,
                         '\02�ȶ�����\18�˸����ֲ����ʽ�',
                         '0001-01-01 00:00:00',
                         'tbSourceForestAid',
                         1,
                         10180,
                         '��',
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
                         '���֤δ�鵽',
                         '���֤δ�鵽',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         10,
                         '���֤δ�鵽',
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
                         '���֤������һ��',
                         '���֤������һ��',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         20,
                         '���֤������һ��',
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
                         '�����',
                         '�˲������',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         30,
                         '�����',
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
                         '���̵Ǽ�',
                         '21���̵Ǽ�����',
                         0,
                         '\02�ȶ�����\21���̵Ǽ�',
                         '0001-01-01 00:00:00',
                         'tbCompareRegister',
                         1,
                         10210,
                         '�й��̵Ǽ�',
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
                         'ҽ��',
                         '15����ҽ�Ʊ�������',
                         0,
                         '\02�ȶ�����\15����ҽ�Ʊ�������',
                         '0001-01-01 00:00:00',
                         'tbCompareMedical',
                         1,
                         10150,
                         'ҽ��',
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
                         '��Ժ��Ϣ',
                         '16��Ժ��Ϣ',
                         0,
                         '\02�ȶ�����\16��Ժ��Ϣ',
                         '0001-01-01 00:00:00',
                         'tbCompareInHospital',
                         1,
                         10160,
                         'סԺ',
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
                         '���̽��赥λ',
                         '20���̽��赥λ��Ϣ',
                         0,
                         '\02�ȶ�����\20���̽��赥λ��Ϣ',
                         '0001-01-01 00:00:00',
                         'tbCompareBulidEnt',
                         1,
                         10200,
                         '����',
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
                         'ƶ����',
                         '00ƶ������Ϣ',
                         0,
                         '\01Դ����\00ƶ������Ϣ',
                         '0001-01-01 00:00:00',
                         'tbSourcePoorInfo',
                         1,
                         0,
                         'ƶ����',
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
                         '�����ƽ�',
                         '01�����ƽ���Ŀ��Ϣ',
                         0,
                         '\01Դ����\01�����ƽ���Ŀ��Ϣ',
                         '0001-01-01 00:00:00',
                         'tbSourceVillageAdv',
                         1,
                         10,
                         '�����ƽ�',
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
                         '�׵ط�ƶ��Ǩ',
                         '02�׵ط�ƶ��Ǩ��Ϣ',
                         0,
                         '\01Դ����\02�׵ط�ƶ��Ǩ��Ϣ',
                         '0001-01-01 00:00:00',
                         'tbSourceMigrate',
                         1,
                         20,
                         '�׵ط�ƶ��Ǩ',
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
                         '����������',
                         '031��������׶μ�ͥ�������Ѽ���������Ѳ���',
                         0,
                         '\01Դ����\031��������׶μ�ͥ�������Ѽ���������Ѳ���',
                         '0001-01-01 00:00:00',
                         'tbSourceBoardAid',
                         1,
                         31,
                         '��',
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
                         '������ѧ��',
                         '032��ͨ���й�����ѧ��',
                         0,
                         '\01Դ����\032��ͨ���й�����ѧ��',
                         '0001-01-01 00:00:00',
                         'tbSourceHighSchoolAd',
                         1,
                         32,
                         '��',
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
                         '��ְ��ѧ��',
                         '033�е�ְҵ����������ѧ��',
                         0,
                         '\01Դ����\033�е�ְҵ����������ѧ��',
                         '0001-01-01 00:00:00',
                         'tbSourceOccEduAd',
                         1,
                         33,
                         '��',
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
                         '��¶�ƻ�',
                         '034��¶�ƻ�',
                         0,
                         '\01Դ����\034��¶�ƻ�',
                         '0001-01-01 00:00:00',
                         'tbSourceRainPlan',
                         1,
                         34,
                         '��',
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
                         '��ƶ�Ŵ�',
                         '04��ƶС���Ŵ��ʽ�',
                         0,
                         '\01Դ����\04��ƶС���Ŵ��ʽ�',
                         '0001-01-01 00:00:00',
                         'tbSourceLoad',
                         1,
                         40,
                         '��',
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
                         '��������',
                         '05������������������',
                         0,
                         '\01Դ����\05������������������',
                         '0001-01-01 00:00:00',
                         'tbSourceMedicalAd',
                         1,
                         50,
                         '��',
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
                         '��ũ��',
                         '06����ũ�����ҽ�ƻ���',
                         0,
                         '\01Դ����\06����ũ�����ҽ�ƻ���',
                         '0001-01-01 00:00:00',
                         'tbSourceVillageMedical',
                         1,
                         60,
                         '��',
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
                         'ְҵ����',
                         '071ְҵ���ܲ���',
                         0,
                         '\01Դ����\071ְҵ���ܲ���',
                         '0001-01-01 00:00:00',
                         'tbSourceJobAd',
                         1,
                         71,
                         '��',
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
                         'ְҵ��ѵ',
                         '072ְҵ��ѵ����',
                         0,
                         '\01Դ����\072ְҵ��ѵ����',
                         '0001-01-01 00:00:00',
                         'tbSourceJobTrainningAd',
                         1,
                         72,
                         '��',
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
                         '��ҵ��ѵ',
                         '073��ҵ��ѵ����',
                         0,
                         '\01Դ����\073��ҵ��ѵ����',
                         '0001-01-01 00:00:00',
                         'tbSourceStartupAd',
                         1,
                         73,
                         '��',
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
                         '���Ѳм��˲���',
                         '081���Ѳм��������',
                         0,
                         '\01Դ����\081���Ѳм��������',
                         '0001-01-01 00:00:00',
                         'tbSourcePoorDisableAd',
                         1,
                         81,
                         '��',
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
                         '�ضȲм��˲���',
                         '082�ضȲм��˻�����',
                         0,
                         '\01Դ����\082�ضȲм��˻�����',
                         '0001-01-01 00:00:00',
                         'tbSourceSevereDisableAd',
                         1,
                         82,
                         '��',
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
                         'ƶ�����ͱ��屣',
                         'ƶ�����ͱ��屣',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         40,
                         '�޵ͱ��屣',
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
                         '�ܽ���',
                         '�˲��ܽ��',
                         0,
                         NULL,
                         NULL,
                         NULL,
                         1,
                         30,
                         '�ܽ��',
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
                         '���ϱ���',
                         '23���ϱ���',
                         0,
                         '\02�ȶ�����\23���ϱ���',
                         '0001-01-01 00:00:00',
                         'tbLifeInsurance',
                         0,
                         10230,
                         '�����ϱ���',
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
                         '����ͨ',
                         '13����ͨ',
                         0,
                         '\01Դ����\13����ͨ',
                         '0001-01-01 00:00:00',
                         'tbTV',
                         1,
                         130,
                         '����',
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
                         '��ƶ����',
                         '12��ƶ�����ʽ�',
                         0,
                         '\01Դ����\12��ƶ�����ʽ�',
                         '0001-01-01 00:00:00',
                         'tbSourcePoorMoney',
                         0,
                         120,
                         '��ƶ�����ʽ�',
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
                         '��С����Ա',
                         '24������С����Ա����',
                         0,
                         '\02�ȶ�����\24������С����Ա����',
                         '0001-01-01 00:00:00',
                         'tbCompareTownFive',
                         1,
                         10240,
                         '��С����Ա��',
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
                         '��С����Ա����',
                         '25������С����Ա��������',
                         0,
                         '\02�ȶ�����\25������С����Ա��������',
                         '0001-01-01 00:00:00',
                         'tbCompareTownFiveRelate',
                         1,
                         10250,
                         '��С����Ա������',
                         'tbCompareTownFiveRelatePre',
                         '',
                         ''
                     );


-- ��ͼ��vw_Region
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
