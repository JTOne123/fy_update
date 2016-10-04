## 编译说明
- 默认仅支持 .Net4.0 如果需要使用其他版本 framework，替换 _DLL 目录中的 SharpCompress.dll 为相应的版本，不保证能编译通过。
- 部署时不需要 SharpCompress.dll 运行时会自动提取。
### Debug
- 仅用来调试，无法实现自更新。
- 调试时,更新目标位于 _Output\Debug\Test 目录下 。
### Release
- 发布版本，可以实现自更新。
- 一般由主程序检测到可用更新后，启动更新程序并将主程序自身的启动路径作为第一个参数传递给更新程序。

## 部署说明
### 部署目录
- 服务端要求使用 packages.txt 存放更新包列表。
- packages.txt 文件跟更新包文件夹必须为同一文件夹。

### 更新包格式
- 更新包为单文件压缩包，支持格式由 SharpCompress 库决定。主要有：zip、rar、7z、gz 等。
- 更新包必须包含新版的主程序，否则会出现循环更新的情况。
- 实际部署的时候扩展名可以带也可不带。如果带扩展名，扩展名不能为纯数字。否则会当作版本号，导致解析错误。
- 压缩时不要在外层嵌套文件夹，直接选中要更新的多个文件压缩即可。
- 更新程序可自更新，所以更新程序可也压缩到更新包中。

### 更新包命名
#### 增量更新包
```
格式：<FROM>_[to_]<TO>[.EXT | _EXT]
```
- <> 表示必选项。[] 表示可选项。
- [.EXT | _EXT] 表示一串用 '.' 或 '__' 连接的字符串，每个子串不能为纯数字。

> 正确：
```
1.0.0_1.0.1
1.0.0_to_1.0.1
1.0.0_1.0.1.7z
1.0.0_1.0.1.a1.7z
1.0.0_1.0.1_a1_zip
```
> 错误：
```
1.0.0-1.1.1                 //版本分割符为半角 ‘_’
1.0.0_1.1.1.zip.77          //[.EXT | _EXT] 有一段为纯数字
1.0.0_1.1.1.zip_77_7z       //同上
```
> 推荐：
```
1.0.0.0_to_1.0.0.1.rar
1.0.0.0_to_1.0.0.1_diff.rar
```

#### 全量更新包
```
格式：<TO>[.EXT | _EXT]
```
- 要求同增量更新包。

> 正确：
```
1.0.1
_1.0.1.7z
1.0.1.a1.7z
1.0.1_a1_zip
```
> 错误：
```
-1.1.1                //版本分割符为半角 ‘_’
1.1.1.zip.77          //[.EXT | _EXT] 有一段为纯数字
1.1.1.zip_77_7z       //同上
```
> 推荐：
```
1.0.0.1.rar
1.0.0.1_full.rar
```

### packages.txt 内容
- 每行一个更新包文件名，必须完全一致，包含扩展名，区分大小写。
- 可以存在空行，空行会被自动忽略。
- 顺序无关，可随意排序。

> 正确
```
1.0.0.1_to_1.0.2.rar
1.0.0.0_to_1.0.1_diff.zip
```

### 更新顺序
- 先查找最新的全量包，如果 TO 比当前版本高，则先下载该包，然后逐个增量包开始更新。
- 如果没有可用的全量包，则直接逐个增量包开始更新。

### 注意事项
- 增量更新包 FROM 必须唯一，但 TO 可以有多个。
- 全量更新包 TO 必须唯一。
- 增量包和全量包之间互不影响。

> 正确：
```
1.0.0.0_to_1.0.3_diff.zip
1.0.0.1_to_1.0.3.rar
```
> 错误：
```
1.0.0.1_to_1.0.3_diff.zip   //增量 FROM 重复
1.0.0.1_to_1.0.2.rar
```
> 错误：
```
1.0.3_full.zip              //全量 TO 重复
1.0.3.7z
```
