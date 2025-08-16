// pch.cpp: 与预编译标头对应的源文件

#include "pch.h"
#include <opencv2/opencv.hpp>
using namespace cv;

// 当使用预编译的头时，需要使用此源文件，编译才能成功。
void Hello()
{
	Mat src = imread("Images/1.jpg", IMREAD_COLOR);
	namedWindow("图像", WINDOW_FREERATIO);
	imshow("图像", src);
}