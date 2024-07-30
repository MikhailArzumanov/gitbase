usermod --login %USERNAME% %PREV_NAME%

mv /home/%PREV_NAME% /home/%USERNAME%
mv /git/%PREV_NAME% /git/%USERNAME%
