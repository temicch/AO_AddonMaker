using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;

namespace AddonElement
{
    [Serializable]
    abstract public class Widget : AddonFile, IUIElement
    {
        [Category("Base properties")]
        [Description("Системное название виджета")]
        public string Name { get; set; }
        [Category("Base properties")]
        [Description("Виден ли виджет. По умолчанию true. если виджет не виден, то он недоступен и для реакций")]
        public bool Visible { get; set; }
        [Category("Base properties")]
        [Description("Доступен ли виджет и все его дочерние виджеты для реакций. Может влиять на внешний вид (виджет \"засеривается\"). По умолчанию true")]
        public bool Enabled { get; set; } = true;
        [Category("Base properties")]
        [Description("Приоритет отображения (также влияет на обработку мышиных событий) виджета в списке виджетов своего родителя. То есть с помощью этого поля можно сформировать иерархию отображения виджетов всего аддона")]
        public int Priority { get; set; }

        [Category("Placement")]
        [Description("Описание расположение виджета")]
        public WidgetPlacementXY Placement { get; set; }

        [Category("Children widgets")]
        [Description("Дочерние виджеты. Почти каждый виджет может содержать дочерние виджеты, за исключением особых случаев типа слайдера и т.п. Дочерние виджеты отображаются поверх родителя и перехватывают реакции (если они объявлены и на них подписаны обработчики) раньше родительского виджета за исключением особых случаев")]
        [XmlArrayItem("Item")]
        public List<href> Children { get; set; }

        [XmlIgnore]
        public List<AddonFile> Widgets
        {
            get => Children?.Select(x => x.File).ToList();
        }

        [Category("Children widgets")]
        [Description("Нужно ли обрезать содержимое, включая дочерние виджеты, по границам данного. По умолчанию false")]
        public bool clipContent { get; set; } = false;

        [Category("Display")]
        [Description("Слой для отображения нижней части текстуры")]
        public href BackLayer { get; set; }
        [Category("Display")]
        [Description("Слой для отображения верхней части текстуры")]
        public href FrontLayer { get; set; }
        [Category("Display")]
        [Description("Текстура с альфой. Используется для задания маски, по которой будет обрезана основная текстура данного контрола и всех его детей")]
        public href textureMask { get; set; }
        [Category("Display")]
        [Description("Визуальная прозрачность виджета. По умолчанию 1.0f - непрозрачен")]
        public float fade { get; set; } = 1f;

        [Category("Track and limit reactions")]
        [Description("Черно-белая текстура (по степеням 2) для задания активной (белые пиксели) области для кликов мышью. Нужно вручную выставлять mipSW = 0 при экспорте")]
        public href pickMask { get; set; }
        [Category("Track and limit reactions")]
        [Description("Обрабатывать мышиные реакции только для детей этого виджета, игнорируя сам виджет")]
        public bool PickChildrenOnly { get; set; }
        [Category("Track and limit reactions")]
        [Description("Игнорировать PickChildrenOnly при скролировании колесом мыши и наведении. Всегда обрабатывать реакцию скролла колесом мыши")]
        public bool forceWheel { get; set; }
        [Category("Track and limit reactions")]
        [Description("Игнорировать двойной клик мышью для виджета и для его детей")]
        public bool IgnoreDblClick { get; set; }
        [Category("Track and limit reactions")]
        [Description("Является ли виджет прозрачным для ввода. По умолчанию false")]
        public bool TransparentInput { get; set; } = false;

        [Category("Special")]
        [Description("Запрещать ли пользовательским аддонам операции с виджетом. По умолчанию false")]
        public bool isProtected { get; set; } = false;
        [Category("Special")]
        [Description("Задаёт порядок обхода контролов по клавише Tab. По умолчанию 0(не учавствует в обходе). Для участия в обходе значение должно быть больше 0")]
        public int TabOrder { get; set; }
        [Category("Special")]
        [Description("Звук на начатое отображение виджета")]
        public href soundShow { get; set; }
        [Category("Special")]
        [Description("Звук на скрытие виджета")]
        public href soundHide { get; set; }

        [Category("Keyboard reactions")]
        [Description("Список реакций на клавиатурные нажатия")]
        public List<BindSection> bindSections { get; set; }

        [Category("Mouse reactions")]
        [Description("Уведомление о наведении на виджет. (Кроме виджетов со специальной обработкой - кнопок и т.д.)")]
        public string reactionOnPointing { get; set; }
        [Category("Mouse reactions")]
        [Description("Уведомление о наведении на виджет вне зависимости от его доступности для кликов. (Использовать только при сильной необходимости - потребляет много ресурсов.)")]
        public string forceReactionOnPointing { get; set; }
        [Category("Mouse reactions")]
        [Description("Уведомление о прокрутке колёсика мыши вверх")]
        public string reactionWheelUp { get; set; }
        [Category("Mouse reactions")]
        [Description("Уведомление о прокрутке колёсика мыши вниз")]
        public string reactionWheelDown { get; set; }

        public ImageSource Bitmap
        {
            get => GetBitmap();
        }

        public Widget()
        {
            Visible = true;
            Enabled = true;
            Priority = 0;
            Placement = new WidgetPlacementXY();

            clipContent = false;
            fade = 1.0f;
            PickChildrenOnly = false;
            forceWheel = false;
            IgnoreDblClick = false;
            TransparentInput = false;

            isProtected = false;
            TabOrder = 0;
        }

        public IEnumerable<AddonFile> GetChildren() => Children.Select(x => x.File);

        protected ImageSource GetBitmap()
        {
            return (BackLayer?.File as WidgetLayer)?.Bitmap;
        }
    }
}
