
��ҵ���c# ���ο���ʾ���ļ�˵��
===========================================================================
�汾�ţ� 4.3.0

֧�������GigE�����U3V���

֧��ϵͳ��windows 7(32��64λ)��windows 10(32��64λ)��windows11(64λ)  

c#֧�����ֶ��ο����ķ�ʽ��
  һ��ֱ�ӵ���dll(MvCameraControl.Net.dll)��
  ����ֱ������Դ���ļ�MVCamera.cs(MVCameraSDK\Source\)��
��ϸ���£�
===========================================================================

..\Samples\C#\MvCameraControlNet Ŀ¼�ṹ���£�
	MVCameraSDK
	|-..\..\..\..\DotNet			������MvCameraControl.Net.dll�ļ���ֱ�ӿ���dll�ļ����ж��ο�����
	|-Source		                ������MVCamera.cs�ļ������ɽ��ж��ο�����
	
	
	
===========================================================================


Machine Vision Camera Windows SDK  c# User Manual
===========================================================================
Version: 4.3.0

Camera supported : GigE and USB3 Camera

OS supported: Windows7(32/64 bits), Windows10 (32/64 bits), windows11 (64 bits)
===========================================================================
  
  
c# supports two ways of Secondary Development
   1.call DLL;
   2.Reference the source code file MVCamera.cs;
Details are as follows:  
===========================================================================

..\Samples\C#\MvCameraControlNet Directory��
	MVCameraSDK
	|-..\..\..\..\DotNet			   :Generate  MvCameraControl.Net.dll��referenced for secondary development��
	|-Source		   
	 -MVCamera.cs                      :MVCamera.cs,Reference this file can be secondary development;
	
	MVCameraSDK_DotNet
	|-Source		   
	 -MVCamera.cs      :MVCamera.cs,Reference this file can be secondary development;
	|- other files     :other files��
	
	
===========================================================================


