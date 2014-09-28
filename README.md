## Image Colors
This program can be used to calculate the RGB and HEX values of JPG and PNG image files.

The challenge during development of this project was giving the CPU a large workload and dealing with it as efficiently as possible.

It has been created as a learning project for the course threading in c#.


### Preview:
![image colors](https://cloud.githubusercontent.com/assets/4095127/4436071/f2c3c5fc-4763-11e4-9d18-0b9bb7aa4dd6.png)

### Features:
 - The ability to calculate the HEX and PNG values of images.
 - Supported file types are: JPG, PNG.
 - The ability to set the number of threads to be used.
 - The ability to assign multiple threads on one image.

### Known Issues:
 - Adding files with a height less than the threads per image breaks the start program.
 - User settings for threads per image and number of cores aren't saved.
