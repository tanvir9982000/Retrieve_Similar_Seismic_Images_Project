clc
clear

img1 = imread('demo1.jpg');
img2 = imread('demo2.jpg');

clear param
param.imageSize = [256 256]; % it works also with non-square images (use the most common aspect ratio in your set)
param.orientationsPerScale = [8 8 8 8]; % number of orientations per scale
param.numberBlocks = 4;
param.fc_prefilt = 4;


%gist1 = LMgist(img1, '', param);
%gist2 = LMgist(img2, '', param);

%D12 = sum((gist1-gist2).^2);

D = img1;
HOMEIMAGES = '';
HOMEGIST = '';
Nscenes = size(D,4);
typeD = 3;

param.boundaryExtension = 32; 

if ~isfield(param, 'G')
    param.G = createGabor(param.orientationsPerScale, param.imageSize+2*param.boundaryExtension);
end

Nfeatures = size(param.G,3)*param.numberBlocks^2;
gist = zeros([Nscenes Nfeatures], 'single');

%n=1

for n = 1:Nscenes
    g = [];
    
    if Nscenes>1 disp([n Nscenes]); end

    % load image
    try
        switch typeD
            case 1
                img = LMimread(D, n, HOMEIMAGES);
            case 2
                img = imread(fullfile(HOMEIMAGES, D{n}));
            case 3
                img = D(:,:,:,n);
        end
    catch
        disp(D(n).annotation.folder)
        disp(D(n).annotation.filename)
        rethrow(lasterror)
    end
    
    % convert to gray scale
    img = single(mean(img,3));

    % resize and crop image to make it square
    img = imresizecrop(img, param.imageSize, 'bilinear');
    
    % scale intensities to be in the range [0 255]
    img = img-min(img(:));
    img = 255*img/max(img(:));
    
    
    output    = prefilt(img, param.fc_prefilt);

    % get gist:
    g = gistGabor(output, param);
    gist(n,:) = g;
    
end



