#!/bin/bash
if test -d  /webapps/bank; 
    then
        echo "/webapps/bank exists";
    else
        echo "/webapps/bank doesn't exist";
        exit 1;
fi

sudo chmod 664 /etc/systemd/system/bank.service
sudo systemctl daemon-reload
sudo systemctl enable bank
sudo systemctl start bank
