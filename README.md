# ElevationHeatMapApp
ShapeFileを読み込み標高をヒートマップで出力するコマンドラインアプリ

## MapConsoleApp
次のようなjsonファイルで色と標高の対応を指定しヒートマップを生成する．
```Gradient.json
{
	"Colors": [
		"#000080",
		"#0000ff",
		"#0080ff",
		"#00ffff",
		"#00ff80",
		"#00ff00",
		"#80ff00",
		"#ffff00",
		"#ff8000",
		"#ff0000"
	],
	"Positions": [
		100,
		200,
		300,
		400,
		500,
		600,
		700,
		800,
		900,
		1000
	]
}
```

![HeatMap](/out/dem.png,"出力されるHeatMap画像")

入力ShapeFileのBounding Box情報も出力できる．

```BoundingBox.csv
dem.png;Env[11563.354895687451 : 23133.76295806214, 92430.53718668334 : 101681.72900453459]
dem2.png;Env[11563.354895687451 : 23133.76295806214, 92430.53718668334 : 101681.72900453459]
```

|Option|Description|
| --- | --- |
|-i —input|入力Shapeファイルのあるディレクトリのパス．|
| -w —width | 出力画像の幅． |
|-h —height|出力画像の高さ．|
|-g —gradient|標高と色を対応させたjsonファイルのパス．|
|-o —output|出力画像とBoundingBox情報の出力ディレクトリパス．|
|-p –interp(optional)|画像の補完機能を有効にする場合は指定する．オプション機能であり，指定は必須ではない．|

# MapConsoleLegend
凡例画像を出力する．
![Legend](/out/Legend.png,"出力される凡例画像")
|Option|Description|
| --- | --- |
|-w —width (optional)|出力画像の幅．指定は任意であり，デフォルト(最小)値は120．|
|-h —height(optional)|出力画像の高さ．指定は任意であり，デフォルト(最小)値は300．|
|-g —gradient|標高と色を対応させたjsonファイルのパス．|
|-o —output|出力画像の出力ディレクトリパス．|
|-n –name(optional)|出力画像の出力ファイル名．指定は任意であり，デフォルトではLegend.pngとして出力される．|
|-a —alpha(optional)|出力画像の背景色を透過する．デフォルトでは白．|




