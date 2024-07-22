usermod --password $(echo %PASSWORD% | openssl passwd -1 -stdin) %USERNAME%
