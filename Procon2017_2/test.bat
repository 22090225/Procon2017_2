@echo off

echo start
rem �J�n���Ԃ̎擾
set b_time=%time%
set b_result=%b_time:~9,2%
set /a b_result=b_result+%b_time:~6,2%*100
set /a b_result=b_result+%b_time:~3,2%*6000
set /a b_result=b_result+%b_time:~0,2%*360000

Procon2017_2.exe < input.txt > output.txt
echo end

rem �I�����Ԃ̎擾
set e_time=%time%
set e_result=%e_time:~9,2%
set /a e_result=e_result+%e_time:~6,2%*100
set /a e_result=e_result+%e_time:~3,2%*6000
set /a e_result=e_result+%e_time:~0,2%*360000

rem ���s���Ԃ̎擾
set /a diff=e_result-b_result
echo %diff%

pause
