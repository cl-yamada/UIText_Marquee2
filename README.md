# UITextMarquee

## | 概要
TextクラスでCanvas上に表示されている文字がスクロールするコンポーネントです。
![marquee_text](https://user-images.githubusercontent.com/93251104/199879945-dc9d378e-6de2-4087-a111-a4823ef17a01.gif)

メッシュの頂点座標を操作して実現しています。

■ Maskなしの見た目
![marquee_text_non_mask](https://user-images.githubusercontent.com/93251104/199880226-756efc9a-5816-427c-9e8f-bb5176d92afc.gif)

## | MarqueeText(Script)
|変数|型|説明|
|-|-|-|
|Space|float|文字がループしたときの末尾と先頭の間隔|
|Speed|float|文字送りの速度|
|UseUnscaledDeltaTIme|bool|チェックを入れるとTimeScaleの影響を受けなくなります|

![image](https://user-images.githubusercontent.com/93251104/199880377-fd1e43a8-1991-4b50-b11d-74d49702926d.png)



## | 参考記事
[【Unity】uGUI の Text をスクロールさせる](https://hacchi-man.hatenablog.com/entry/2020/01/30/220000)
