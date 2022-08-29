using Amazon.CDK;
using Constructs;

namespace Bankinfra
{
    public class BankinfraStack : Stack
    {
        internal BankinfraStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            Vpc vpc = new Vpc(this,"bank_vpc");
            LinuxUserDataOptions userData = new LinuxUserDataOptions()
            {
                Shebang = "#!/bin/bash"
            };
            Instance_  ec2 = new Instance_(this, "bank_ec2", 
                new InstanceProps(){
                    InstanceType = new InstanceType("t2.micro"),
                    MachineImage = MachineImage.LatestAmazonLinux(new AmazonLinuxImageProps(){Generation = AmazonLinuxGeneration.AMAZON_LINUX_2}),
                    Vpc = vpc,
                    VpcSubnets = new SubnetSelection()
                    {
                        SubnetType = SubnetType.PUBLIC
                    },
                    UserData = UserData.ForLinux(userData),
                });
            ec2.Connections.AllowFromAnyIpv4(Port.Tcp(5000), "Allow Inbound HTTP Request");
            ec2.UserData.AddCommands("yum update");
            ec2.UserData.AddCommands("yum install -y ruby");
            ec2.UserData.AddCommands("yum install -y wget");
            ec2.UserData.AddCommands("cd /home/ec2-user");
            ec2.UserData.AddCommands("wget https://aws-codedeploy-us-east-1.s3.us-east-1.amazonaws.com/latest/install");
            ec2.UserData.AddCommands("chmod +x ./install");
            ec2.UserData.AddCommands("./install auto");
            ec2.UserData.AddCommands("service codedeploy-agent start");
        }
    }
}
