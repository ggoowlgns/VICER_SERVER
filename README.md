# VICER_SERVER

## 소개
컨트롤러와 카메라에서 정보를 받아 클라이언트에게 전달해주는 server

#### Server 기능
* 클라이언트가 작동시키는 컨트롤러의 정보를 받아 차량에서 정보를 전송<br/>
* 카메라의 영상을 받아 client가 볼 수 있도록 영상을 전송

## 개발환경 설정 실행환경
AWS(Amazon Web Service) EC2와 python을 이용하여 Server를 구현하였다.

### AWS EC2 (Amazon Web Service Elastic Compute cloud)
* 아마존에서 제공하는 클라우드 컴퓨팅 서비스이다. 가입후 1년간 무료로 사용이 가능하다. 아래 링크에서 가입하도록 한다.
   * [가입] http://aws.amazon.com/free/<br/>
* 아래의 링크를 들어가면 현재 자신이 운용하고있는 EC2 머신들의 상태를 확인하고 관리 할 수 있다. 
   * [대시보드] https://ap-northeast-2.console.aws.amazon.com/ec2/v2/home?region=ap-northeast-2#Home:
* 이제 아마존에서 제공하는 공식 Document를 참조하여 서버를 운용하게 될 Cloud Computer를 생성하도록 한다. 서버를 돌려야하므로 리눅스 운영체제를 선택한다.
   * [EC2 생성하기] https://docs.aws.amazon.com/ko_kr/AWSEC2/latest/UserGuide/concepts.html
* EC2는 Cloud Computing을 이용한 가상 머신으로써 이에 접근 하려면 Public IP를 사용하여야 접근이 가능하다. 하지만 처음 EC2 인스턴스를 생성하게 되었을때 주어지는 Public IP는 접근성이 떨어지는 이메일 형식으로 되어있다.
   * [탄력적 IP] https://docs.aws.amazon.com/ko_kr/AWSEC2/latest/UserGuide/elastic-ip-addresses-eip.html
* 마지막으로 방금 생성하게된 EC2 인스턴스는 GUI환경이 아닌 리눅스 운영체제이다. 만들어진 인스턴스에 쉽게 접근하고 작업하기 위하여 Putty를 이용 할 것이다.(밑에서 자세한 설치 및 사용 방법을 기재 할 것이다.) 이를 위해서는 SSH 프로토콜 방식으로 제어를 해야하는데 

    





### Python
* 읽고 사용하기쉬운 Python을 사용하여 구현하였다.<br/>
Python은 위의 AWS EC2 가상머신을 리눅스 운영체제로 구축할 경우 이미 설치 되어 있다.

### Putty
* Server를 구동하기위해 가상머신 Putty를 설치해줘야 한다.


