모바일 초대장 리소스 정리 프로그램을 로컬에서 디버그시 지침

모초는 SMB 파일을 로컬에 마운트 해야 하기 때문에 파일 공유 볼륨을 생성하고 실행시 마운트 해야 됨.
** 로컬 디버그시 WSL 환경을 사용 **

로컬 WSL에서 볼륨 마운트
sudo mkdir /mnt/mcardshare
if [ ! -d "/etc/smbcredentials" ]; then
    sudo mkdir /etc/smbcredentials
fi
if [ ! -f "/etc/smbcredentials/barunprivatestorage.cred" ]; then
    sudo bash -c 'echo "username=barunprivatestorage" >> /etc/smbcredentials/barunprivatestorage.cred'
    sudo bash -c 'echo "password=<azure portal에서 확인>" >> /etc/smbcredentials/barunprivatestorage.cred'
fi
sudo chmod 600 /etc/smbcredentials/barunprivatestorage.cred

sudo bash -c 'echo "//172.16.4.4/mcardshare /mnt/mcardshare cifs nofail,credentials=/etc/smbcredentials/barunprivatestorage.cred,dir_mode=0777,file_mode=0777,serverino,nosharesock,actimeo=30" >> /etc/fstab'
sudo mount -t cifs //172.16.4.4/mcardshare /mnt/mcardshare -o credentials=/etc/smbcredentials/barunprivatestorage.cred,dir_mode=0777,file_mode=0777,serverino,nosharesock,actimeo=30

** 로컬 디버그시 WSL 환경을 사용 **



** Docker 사용시 (Key vault 사용시는 인증을 따로 구현해야 함) **
볼륨 생성 명령어 예) 
아래 명령어는 운영 경로로 테스트시 저적한 값으로 변경하여 마운트
username은 account name, password는 Account keys를 입력

docker volume create \
--driver local \
--opt type=cifs \
--opt device=//barunprivatestorage.file.core.windows.net/mcardshare \
--opt o=addr=barunprivatestorage.file.core.windows.net,username=barunprivatestorage,password=*****==,file_mode=0777,dir_mode=0777 \
--name mcardsharevolume

사무실에서 연결시
docker volume create \
--driver local \
--opt type=cifs \
--opt device=//172.16.4.4/mcardshare \
--opt o=addr=172.16.4.4,username=barunprivatestorage,password=******,file_mode=0777,dir_mode=0777 \
--name mcardsharevolume


Docker 프로파일에서 DockerfileRunArguments 값에 아래 내용이 없을 경우 추가
-v mcardsharevolume:/mnt/mcardshare

launchSettings.json 예)
{
  "profiles": {
    "Barunson.WorkerService.MCardResourceCleanJob": {
      "commandName": "Project",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development"
      },
      "dotnetRunMessages": true
    },
    "Docker": {
      "commandName": "Docker",
      "DockerfileRunArguments": "-v mcardsharevolume:/mnt/mcardshare "
    }
  }
}

