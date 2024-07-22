usermod --login %USERNAME% %PREV_NAME%

mv -r /home/%PREV_NAME% /home/%USERNAME%
mv -r /git/%PREV_NAME% /git/%USERNAME%
