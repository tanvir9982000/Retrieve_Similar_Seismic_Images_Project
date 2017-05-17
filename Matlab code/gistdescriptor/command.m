
%Load images
img1 = imread('demo1.jpg');
%whos img1

img2 = imread('demo2.jpg');
img3 = imread('demo3.jpg');
img4 = imread('demo4.jpg');

% GIST Parameters:
clear param
param.imageSize = [256 256]; % it works also with non-square images (use the most common aspect ratio in your set)
param.orientationsPerScale = [8 8 8 8]; % number of orientations per scale
param.numberBlocks = 4;
param.fc_prefilt = 4;

% Computing gist:
gist1 = LMgist(img1, '', param);
gist2 = LMgist(img2, '', param);
gist3 = LMgist(img3, '', param);
gist4 = LMgist(img4, '', param);

%compare Distance between the three images (More the match, Less the D value):
D12 = sum((gist1-gist2).^2)
D13 = sum((gist1-gist3).^2)
D14 = sum((gist1-gist4).^2)



%{
clc
clear
img = imread('demo1.jpg');
param.boundaryExtension = 32;
param.imageSize = 128;
param.orientationsPerScale = [8 8 8 8];
param.numberBlocks = 4;
param.fc_prefilt = 4;

param.G = createGabor(param.orientationsPerScale, param.imageSize+2*param.boundaryExtension);
%}




