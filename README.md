# KRSFinder

**如果这个项目对您有用,请点击Star**  
## 项目介绍
该项目是一个用以对抗ProGuard混淆规则的工具  
现有的apk基本上都被ProGuard所保护,所有类,成员,方法全部被混淆为无意义的短字母  
该项目可以通过提取在smali层残留下来的信息用以帮助逆向人员对抗这类混淆  

* 那些信息?
  * 提取被混淆的原类名 `.source "XXX.java"`
  * 提取全局变量在某些方法内的局部变量变量名 `String someName = this.a;`
  * 提取全局变量曾被赋予那些常量 `a = "const";`
  * 提取全局变量曾被赋予那些方法的返回数据 `a = new SomeClass().someMethod();`
  * 提取全局变量曾被那些其他变量所赋值 `a = new SomeClass().b;`

逆向人员可以根据提取到的信息判断被混淆成员与成员间的关系,  
或被混淆成员原来的成员名,及其原有作用  
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

**是的,如这些截图所见,该项目是完全可以分析这类代码的  
该工具会自动遍历所有可能产生的分支,  
即使是比以上更加复杂的分支和场景,项目都可以分析,  
并尽可能收集所有可以用以对抗ProGuard的信息**  

这是我的第一个C#项目,断网15天,终于在今天完成了:)  
如果您喜欢这个项目,请给我一个Star:)  

This is my blog  
https://www.cnblogs.com/aldys4/  

其他项目截图如下  

![KRS](https://user-images.githubusercontent.com/89259981/139191644-6ac1ce00-4343-4a4c-8979-c95dcd2d7fb4.png)  
![KRS1](https://user-images.githubusercontent.com/89259981/139191653-ef00f37a-b299-4c43-b46d-21849e457240.png)

