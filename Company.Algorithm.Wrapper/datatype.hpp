#pragma once

#include <opencv2/opencv.hpp>

namespace company
{
	//ͼ��ģ��
	template <typename _T, int _channel = 1>
	struct _Image
	{
		_T* data; //ͼ���ָ��
		int width; //ͼ��Ŀ��
		int height; //ͼ��ĸ߶�
	};

	typedef _Image<uchar, 1> Image8UC1; //8λ��ͨ��ͼ��
	typedef _Image<uchar, 3> Image8UC3; //24λ��ͨ��ͼ��
	typedef _Image<uchar, 4> Image8UC4; //32λ��ͨ��ͼ��
	typedef _Image<ushort, 1> Image16UC1; //16λ��ͨ��ͼ��
	typedef _Image<float, 1> Image32FC1; //32λ��ͨ��ͼ��

	struct ColorBGRA
	{
		uchar B;
		uchar G;
		uchar R;
		uchar A;
	};

	template <typename _T, int _channel = 1>
	__declspec(dllexport) cv::Mat CppImageToMat(const _Image<_T, _channel>& img)
	{
		if (typeid(_T) == typeid(uchar) && _channel == 1)
			return cv::Mat(img.height, img.width, CV_8UC1, img.data);
		else if (typeid(_T) == typeid(uchar) && _channel == 3)
			return cv::Mat(img.height, img.width, CV_8UC3, img.data);
		else if (typeid(_T) == typeid(uchar) && _channel == 4)
			return cv::Mat(img.height, img.width, CV_8UC4, img.data);
		else if (typeid(_T) == typeid(ushort) && _channel == 1)
			return cv::Mat(img.height, img.width, CV_16UC1, img.data);
		else if (typeid(_T) == typeid(float) && _channel == 1)
			return cv::Mat(img.height, img.width, CV_32FC1, img.data);

		return cv::Mat();
	}
}