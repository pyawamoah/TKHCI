select * from TitheTB
select * from ServiceOfferingTB
select * from PledgeTB

select SUM(T_Amount) as TotalTithe from TitheTB

select SUM(MW_Offering) as MWO, SUM(SF_Offering) as SFO, SUM(SS_Offering) as SSO from ServiceOfferingTB

select SUM(PP_Amount) as PledgeAmt from PledgeTB

select T_Mem_ID, T_Mem_Name, T_Cur_Type, T_Pay_Channel, T_Amount  from TitheTB ORDER BY
T_Mem_ID


 SELECT T_Mem_ID, T_Mem_Name, T_Cur_Type, T_Pay_Channel, T_Amount,
  SUM(T_Amount) OVER(ORDER BY T_Mem_ID 
     ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) 
          AS RunningTotal
  FROM TitheTB


--group by T_Mem_ID, T_Mem_Name, T_Cur_Type, T_Chq_No, T_eTran_No, T_Amount

