// pch.cpp: 与预编译标头对应的源文件

#include "pch.h"

// 当使用预编译的头时，需要使用此源文件，编译才能成功。

#include <opencv2/opencv.hpp>
using namespace company;
using namespace cv;

static void Hello() {
	Mat src = imread("Images/1.jpg", IMREAD_COLOR);
	namedWindow("图像", WINDOW_FREERATIO);
	imshow("图像", src);
}

//数组访问
static void CppTest(const Image16UC1& src_image, Image16UC1 dst_image, ushort v) {
	Mat src = CppImageToMat(src_image);
	Mat dst = CppImageToMat(dst_image);

	//数组访问
	for (size_t i = 0; i < src_image.height; i++)
	{
		for (size_t j = 0; j < src_image.width; j++)
		{
			int pv = dst.at<ushort>(i, j);//获取某行某行的像素点
			dst.at<ushort>(i, j) = v + pv;
		}
	}
}

//索贝尔算法
static void CppSobel(const Image16UC1& src_image, Image16UC1 dst_image, int v) {
	if (v % 2 == 0) return;
	Mat src = CppImageToMat(src_image);
	Mat dst = CppImageToMat(dst_image);

	Mat matx;
	Mat maty;

	Sobel(src, matx, CV_64F, 1, 0, v);
	Sobel(src, maty, CV_64F, 1, 0, v);

	Mat result;
	magnitude(matx, maty, result);
	result.convertTo(src, src.type());
}

//中值滤波器算法
static void CppMedianBlur(const Image16UC1& src_image, Image16UC1 dst_image, int v) {
	Mat src = CppImageToMat(src_image);
	Mat dst = CppImageToMat(dst_image);

	medianBlur(src, dst, v);
}
