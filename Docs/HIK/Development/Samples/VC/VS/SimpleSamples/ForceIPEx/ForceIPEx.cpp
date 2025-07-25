﻿#include <stdio.h>
#include <Windows.h>
#include <process.h>
#include <conio.h>
#include "MvCameraControl.h"
#pragma comment(lib, "wsock32.lib")

#define PERSISTENT_CONFIG_REG 0x0014
#define PERSISTENT_IPADDR_REG 0x064C
#define PERSISTENT_SUBNETMASK_REG 0x065C
#define PERSISTENT_DEFAULTGATEWAY_REG 0x066C

// ch:等待按键输入 | en:Wait for key press
void WaitForKeyPress(void)
{
	while(!_kbhit())
	{
		Sleep(10);
	}
	_getch();
}

bool PrintDeviceInfo(MV_CC_DEVICE_INFO* pstMVDevInfo)
{
	if (NULL == pstMVDevInfo)
	{
		printf("The Pointer of pstMVDevInfo is NULL!\n");
		return false;
	}
	if (pstMVDevInfo->nTLayerType == MV_GIGE_DEVICE)
	{
		int nIp1 = ((pstMVDevInfo->SpecialInfo.stGigEInfo.nCurrentIp & 0xff000000) >> 24);
		int nIp2 = ((pstMVDevInfo->SpecialInfo.stGigEInfo.nCurrentIp & 0x00ff0000) >> 16);
		int nIp3 = ((pstMVDevInfo->SpecialInfo.stGigEInfo.nCurrentIp & 0x0000ff00) >> 8);
		int nIp4 = (pstMVDevInfo->SpecialInfo.stGigEInfo.nCurrentIp & 0x000000ff);

		// ch:打印当前相机ip和用户自定义名字 | en:print current ip and user defined name
		printf("CurrentIp: %d.%d.%d.%d\n" , nIp1, nIp2, nIp3, nIp4);
		printf("UserDefinedName: %s\n\n" , pstMVDevInfo->SpecialInfo.stGigEInfo.chUserDefinedName);
	}
	else if (pstMVDevInfo->nTLayerType == MV_USB_DEVICE)
	{
		printf("UserDefinedName: %s\n", pstMVDevInfo->SpecialInfo.stUsb3VInfo.chUserDefinedName);
		printf("Serial Number: %s\n", pstMVDevInfo->SpecialInfo.stUsb3VInfo.chSerialNumber);
		printf("Device Number: %d\n\n", pstMVDevInfo->SpecialInfo.stUsb3VInfo.nDeviceNumber);
	}
	else
	{
		printf("Not support.\n");
	}

	return true;
}

//地址的转换函数
bool ConvertToHexIp(unsigned int *nHexIP, unsigned int *nDecIP, char c)
{
	if ( nDecIP[0] < 0 || nDecIP[0] > 255
		|| nDecIP[1] < 0 || nDecIP[1] > 255
		|| nDecIP[2] < 0 || nDecIP[2] > 255
		|| nDecIP[3] < 0 || nDecIP[3] > 255
		|| c != '\n')
	{
		return false;
	}
	*nHexIP = (nDecIP[0] << 24) + (nDecIP[1] << 16) + (nDecIP[2] << 8) + nDecIP[3];

	return true;
}
int main()
{
	int nRet = MV_OK;
	void* handle = NULL;
	unsigned int nIP[4] = {0};
	char c = '\0';
	unsigned int nIpAddr = 0, nNetWorkMask = 0, nDefaultGateway = 0;
	do 
	{
		// ch:初始化SDK | en:Initialize SDK
		nRet = MV_CC_Initialize();
		if (MV_OK != nRet)
		{
			printf("Initialize SDK fail! nRet [0x%x]\n", nRet);
			break;
		}

		// ch:枚举设备 | en:Enum device
		MV_CC_DEVICE_INFO_LIST stDeviceList;
		memset(&stDeviceList, 0, sizeof(MV_CC_DEVICE_INFO_LIST));
		nRet = MV_CC_EnumDevices(MV_GIGE_DEVICE | MV_USB_DEVICE, &stDeviceList);
		if (MV_OK != nRet)
		{
			printf("Enum Devices fail! nRet [0x%x]\n", nRet);
			break;
		}

		if (stDeviceList.nDeviceNum > 0)
		{
			for (unsigned int i = 0; i < stDeviceList.nDeviceNum; i++)
			{
				printf("[device %d]:\n", i);
				MV_CC_DEVICE_INFO* pDeviceInfo = stDeviceList.pDeviceInfo[i];
				if (NULL == pDeviceInfo)
				{
					break;
				} 
				PrintDeviceInfo(pDeviceInfo);            
			}  
		} 
		else
		{
			printf("Find No Devices!\n");
			break;
		}

		printf("Please Input camera index(0-%d):", stDeviceList.nDeviceNum-1);
		unsigned int nIndex = 0;
		scanf_s("%d", &nIndex);

		if (nIndex >= stDeviceList.nDeviceNum)
		{
			printf("Input error!\n");
			break;
		}

		// ch:选择设备并创建句柄 | en:Select device and create handle
		nRet = MV_CC_CreateHandle(&handle, stDeviceList.pDeviceInfo[nIndex]);
		if (MV_OK != nRet)
		{
			printf("Create Handle fail! nRet [0x%x]\n", nRet);
			break;
		}

		//记录第一次的序列号
		//Record the serial number for the first time
		char chSerialNumber[16] = {0};
		memcpy(chSerialNumber, stDeviceList.pDeviceInfo[nIndex]->SpecialInfo.stGigEInfo.chSerialNumber, sizeof(stDeviceList.pDeviceInfo[nIndex]->SpecialInfo.stGigEInfo.chSerialNumber));
		printf("serial number:[%s]\n", chSerialNumber);

		// 输入IP 子网掩码 默认网关
		// input ip, subnet mask and defaultway
		printf("Please input ip, example: 192.168.1.100\n");

		if ( 5 != scanf("%d.%d.%d.%d%c", &nIP[0], &nIP[1], &nIP[2], &nIP[3], &c) )
		{
			printf("input count error\n");
			break;
		}
		if (!ConvertToHexIp(&nIpAddr, nIP, c))
		{
			printf("input IpAddr format is not correct\n");
			break;
		}

		printf("Please input NetMask, example: 255.255.255.0\n");
		if ( 5 != scanf("%d.%d.%d.%d%c", &nIP[0], &nIP[1], &nIP[2], &nIP[3], &c) )
		{
			printf("input count error\n");
			break;
		}
		if (!ConvertToHexIp(&nNetWorkMask, nIP, c))
		{
			printf("input NetMask format is not correct\n");
			break;
		}

		printf("Please input DefaultWay, example: 192.168.1.1\n");
		if ( 5 != scanf("%d.%d.%d.%d%c", &nIP[0], &nIP[1], &nIP[2], &nIP[3], &c) )
		{
			printf("input count error\n");
			break;
		}
		if (!ConvertToHexIp(&nDefaultGateway, nIP, c))
		{
			printf("input DefaultWay format is not correct\n");
			break;
		}

		//判断设备Ip是否可达
		//Determine whether the IP address is reachable
		bool bAccessible = MV_CC_IsDeviceAccessible(stDeviceList.pDeviceInfo[nIndex], MV_ACCESS_Exclusive);
		if(bAccessible)
		{
			//ch:设置IP配置选项 | en:set IP config
			nRet = MV_GIGE_SetIpConfig(handle, MV_IP_CFG_STATIC);
			if (MV_OK != nRet)
			{
				printf("MV_GIGE_SetIpConfig fail! nRet [%x]\n", nRet);
				break;
			}
			printf("set IPConfig succeed\n");

			//ch:设置强制IP | en:set force IP
			nRet = MV_GIGE_ForceIpEx(handle, nIpAddr, nNetWorkMask, nDefaultGateway);
			if (MV_OK != nRet)
			{	
				printf("MV_GIGE_ForceIpEx fail! nRet [%x]\n", nRet);
				break;
			}
			printf("set IP succeed\n");
		}
		else
		{
			//ch:设置强制IP | en:set force IP
			nRet = MV_GIGE_ForceIpEx(handle, nIpAddr, nNetWorkMask, nDefaultGateway);
			if (MV_OK != nRet)
			{	
				printf("MV_GIGE_ForceIpEx fail! nRet [%x]\n", nRet);
				break;
			}
			printf("set IP succeed\n");

			MV_CC_DestroyHandle(handle);
			handle = NULL;

			//ch:需要重新创建句柄，设置为静态IP方式进行保存 | en:re-create the handle and set it to static IP for storage
			stDeviceList.pDeviceInfo[nIndex]->SpecialInfo.stGigEInfo.nCurrentIp = nIpAddr;
			stDeviceList.pDeviceInfo[nIndex]->SpecialInfo.stGigEInfo.nCurrentSubNetMask = nNetWorkMask;
			stDeviceList.pDeviceInfo[nIndex]->SpecialInfo.stGigEInfo.nDefultGateWay = nDefaultGateway;
			nRet = MV_CC_CreateHandle(&handle, stDeviceList.pDeviceInfo[nIndex]);
			if (MV_OK != nRet)
			{
				printf("MV_CC_CreateHandle fail! nRet [%x]\n", nRet);
				break;
			}
			// ch:设置IP配置选项 | en:set IP config
			nRet = MV_GIGE_SetIpConfig(handle, MV_IP_CFG_STATIC);
			if (MV_OK != nRet)
			{
				printf("MV_GIGE_SetIpConfig fail! nRet [%x]\n", nRet);
				break;
			}
			printf("set IPConfig succeed\n");
		}
		//ch:销毁句柄 | en:Destroy handle
		nRet = MV_CC_DestroyHandle(handle);
		if (MV_OK != nRet)
		{
			printf("Destroy Handle fail! nRet [0x%x]\n", nRet);
			break;
		}
		handle = NULL;

		//设置完forceip后重新枚举一下 | en:After setting up ForceIP, re-enumerate it
		Sleep(500);
		nIndex = -1;
		memset(&stDeviceList, 0, sizeof(MV_CC_DEVICE_INFO_LIST));
		nRet = MV_CC_EnumDevices(MV_GIGE_DEVICE | MV_USB_DEVICE, &stDeviceList);
		if (MV_OK != nRet)
		{
			printf("Enum Devices fail! nRet [0x%x]\n", nRet);
			break;
		}

		if (stDeviceList.nDeviceNum > 0)
		{
			for (unsigned int i = 0; i < stDeviceList.nDeviceNum; i++)
			{
				
				MV_CC_DEVICE_INFO* pDeviceInfo = stDeviceList.pDeviceInfo[i];
				if (NULL == pDeviceInfo)
				{
					printf("pointer invalid!\n");
					break;
				} 
				
				if (!strcmp(chSerialNumber, (char*)(pDeviceInfo->SpecialInfo.stGigEInfo.chSerialNumber)))
				{
					nIndex = i;
					printf("find it, serial number:[%s]\n", pDeviceInfo->SpecialInfo.stGigEInfo.chSerialNumber);
					break;
				}
			}  
		} 
		else
		{
			printf("Find No Devices!\n");
			break;
		}
		if (-1 == nIndex)
		{
			printf("Not Find First Device\n");
		}

		// ch:选择设备并创建句柄 | en:Select device and create handle
		nRet = MV_CC_CreateHandle(&handle, stDeviceList.pDeviceInfo[nIndex]);
		if (MV_OK != nRet)
		{
			printf("Create Handle fail! nRet [0x%x]\n", nRet);
			break;
		}

		// ch:打开设备 | en:Open device
		nRet = MV_CC_OpenDevice(handle);
		if (MV_OK != nRet)
		{
			printf("Open Device fail! nRet [0x%x]\n", nRet);
			break;
		}
		//ch:设置presistent IP | en: set presistent IP
		unsigned int nConfig = htonl(0x05);
		nRet = MV_CC_WriteMemory(handle, (const void*)&(nConfig), PERSISTENT_CONFIG_REG, sizeof(nConfig));
		if (MV_OK != nRet)
		{
			printf("write config fail! nRet [0x%x]\n", nRet);
			break;
		}
		else
		{
			printf("write config success! nRet [0x%x]\n", nRet);
		}
		unsigned int nIpAddr1 = htonl(nIpAddr);
		nRet = MV_CC_WriteMemory(handle, (const void*)&(nIpAddr1), PERSISTENT_IPADDR_REG, sizeof(nIpAddr));
		if (MV_OK != nRet)
		{
			printf("write ip fail! nRet [0x%x]\n", nRet);
			break;
		}
		unsigned int nNetWorkMask1 = htonl(nNetWorkMask);
		nRet = MV_CC_WriteMemory(handle, (const void*)&(nNetWorkMask1), PERSISTENT_SUBNETMASK_REG, sizeof(nNetWorkMask));
		if (MV_OK != nRet)
		{
			printf("write nNetWorkMask fail! nRet [0x%x]\n", nRet);
			break;
		}
		unsigned int nDefaultGateway1 = htonl(nDefaultGateway);
		nRet = MV_CC_WriteMemory(handle, (const void*)&(nDefaultGateway1), PERSISTENT_DEFAULTGATEWAY_REG, sizeof(nDefaultGateway));
		if (MV_OK != nRet)
		{
			printf("write nDefaultGateway fail! nRet [0x%x]\n", nRet);
			break;
		}
		// ch:关闭设备 | Close device
		nRet = MV_CC_CloseDevice(handle);
		if (MV_OK != nRet)
		{
			printf("Close device fail! nRet [0x%x]\n", nRet);
			break;
		}
		// ch:销毁句柄 | Destroy handle
		nRet = MV_CC_DestroyHandle(handle);
		if (MV_OK != nRet)
		{
			printf("Destroy Handle fail! nRet [0x%x]\n", nRet);
			break;
		}
		handle = NULL;
		printf("ForceIp Success!\n");
	} while (0);

	if (handle != NULL)
	{
		MV_CC_DestroyHandle(handle);
		handle = NULL;
	}
	

	// ch:反初始化SDK | en:Finalize SDK
	MV_CC_Finalize();

	printf("Press a key to exit.\n");
	WaitForKeyPress();

	return 0;
}
