# KRSFinder

**如果这个项目对您有用,请点击Star**  
## 项目介绍
该项目是一个用以对抗ProGuard混淆规则的工具  
现有的apk基本上都被ProGuard所保护,所有类,成员,方法全部被混淆为无意义的短字母  
该项目可以通过提取在smali层残留下来的信息用以帮助逆向人员对抗这类混淆  
**目前该项目已利用JavaAgent技术实现与JADX联动**   

**关于本项目的更多细节:https://www.cnblogs.com/aldys4/p/16325580.html**

* 那些信息?
  * 提取被混淆的原类名 `.source "XXX.java"`
  * 提取全局变量在某些方法内的局部变量变量名 `String someName = this.a;`
  * 提取全局变量曾被赋予那些常量 `a = "const";`
  * 提取全局变量曾被赋予那些方法的返回数据 `a = new SomeClass().someMethod();`
  * 提取全局变量曾被那些其他变量所赋值 `a = new SomeClass().b;`

* 与JADX联动实现了那些功能?
  * 可标记函数参数为污点,并分析函数参数的传播方向,对着目标函数按'c'即可显示函数方法的传播方向   
  * 可在JADX内显示KRSFinder提取出的节点信息,对着目标节点按'm'即可.  

逆向人员可以根据提取的信息初步判断函数功能,成员间关系.
## 技术亮点  

项目可以在那些情况下提取信息?  
假设您想分析`SomeClass.b`这个成员的信息  
比如该成员曾被赋予那些常量  
这里例举以下几个Java代码的片段转换为Smali代码时被项目分析的场景  
```java
String a = "const";
if(Some_expression()){ SomeClass.b = a; }
```
![KRS2](https://user-images.githubusercontent.com/89259981/139191385-c51b8a03-8c3f-45ab-a027-9ecfb076fb92.png)

```java
String a = SomeClass.b;
String c = "some_useless_word";
switch(some_key){
  case ...; 
  case XXX:
   c = "Key_Word!!";
  break;
  case ...;   
}
a = c;
```
![KRS3](https://user-images.githubusercontent.com/89259981/139191583-f4282fb7-d2ff-40af-8211-aa6ad994d818.png)
```java
String a = SomeClass.b;
String c = "useless_word";
do{
a = c;
c = "Real_Key_Word!";
}while(true);
```
![KRS4](https://user-images.githubusercontent.com/89259981/139191614-c167cab3-d4ae-4ef1-ab08-3222d58bee1d.png)

**是的,如这些截图所见,该项目是可以分析这类代码的  
该工具会自动遍历大部分可能产生的分支,  
即使是比以上更加复杂的分支和场景,项目都可以分析,  
并尽可能收集所有可以用以对抗ProGuard的信息**   

## 使用说明   
工具内第一个'Path'输入栏用于输入Apktool反编译Apk输出的根目录.   
接着点击'KRS Start'即可开始分析函数调用栈,若勾选下方的'获取成员间关系'选项,   
就可以分析成员间关系.      
分析完成后点击'JADX Start',将出现一个基本配置框,   
'Jadx root path'栏输入Jadx的根目录(注意,必须为Jar版本的JADX,exe版本的JADX不支持!)   
剩下两个输入栏分别为callstack.json与result.json(即分析生成的结果文件)的默认路径   
接着点击下方的Start按钮即可联动JADX.   

![JADX-PLUG](https://user-images.githubusercontent.com/89259981/169800658-77955e9d-5c94-4980-8e81-fa5440a14985.png)

![57a1c809fc4ac4d4977083a81114ad1bbc190296](https://user-images.githubusercontent.com/89259981/169809694-6bf42d1e-6999-4331-9cbf-96020f707a81.gif)

This is my blog  
https://www.cnblogs.com/aldys4/  

其他项目截图如下  

![KRS](https://user-images.githubusercontent.com/89259981/139191644-6ac1ce00-4343-4a4c-8979-c95dcd2d7fb4.png)  
![KRS1](https://user-images.githubusercontent.com/89259981/139191653-ef00f37a-b299-4c43-b46d-21849e457240.png)

