using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;

namespace AddonElement
{
    [Serializable]
    public abstract class Widget : File, IUIElement
    {
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

        [XmlElement("Visible")]
        public string _Visible
        {
            get => Visible.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    Visible = result;
            }
        }

        [XmlElement("Enabled")]
        public string _Enabled
        {
            get => Enabled.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    Enabled = result;
            }
        }

        [Category("Base properties")]
        [Description(
            "Приоритет отображения (также влияет на обработку мышиных событий) виджета в списке виджетов своего родителя. То есть с помощью этого поля можно сформировать иерархию отображения виджетов всего аддона")]
        public int Priority { get; set; }

        [Category("Children widgets")]
        [Description(
            "Дочерние виджеты. Почти каждый виджет может содержать дочерние виджеты, за исключением особых случаев типа слайдера и т.п. Дочерние виджеты отображаются поверх родителя и перехватывают реакции (если они объявлены и на них подписаны обработчики) раньше родительского виджета за исключением особых случаев")]
        [XmlArrayItem("Item")]
        public List<href> Children { get; set; }

        [Category("Children widgets")]
        [Description("Нужно ли обрезать содержимое, включая дочерние виджеты, по границам данного. По умолчанию false")]
        [XmlIgnore]
        public bool clipContent { get; set; }

        [XmlElement("clipContent")]
        public string _clipContent
        {
            get => clipContent.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    clipContent = result;
            }
        }

        [Category("Display")]
        [Description("Слой для отображения нижней части текстуры")]
        public href BackLayer { get; set; }

        [Category("Display")]
        [Description("Слой для отображения верхней части текстуры")]
        public href FrontLayer { get; set; }

        [Category("Display")]
        [Description(
            "Текстура с альфой. Используется для задания маски, по которой будет обрезана основная текстура данного контрола и всех его детей")]
        public href textureMask { get; set; }

        [Category("Display")]
        [Description("Визуальная прозрачность виджета. По умолчанию 1.0f - непрозрачен")]
        public float fade { get; set; } = 1f;

        [Category("Track and limit reactions")]
        [Description(
            "Черно-белая текстура (по степеням 2) для задания активной (белые пиксели) области для кликов мышью. Нужно вручную выставлять mipSW = 0 при экспорте")]
        public href pickMask { get; set; }

        [Category("Track and limit reactions")]
        [Description("Обрабатывать мышиные реакции только для детей этого виджета, игнорируя сам виджет")]
        [XmlIgnore]
        public bool PickChildrenOnly { get; set; }

        [XmlElement("PickChildrenOnly")]
        public string _PickChildrenOnly
        {
            get => PickChildrenOnly.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    PickChildrenOnly = result;
            }
        }

        [Category("Track and limit reactions")]
        [Description(
            "Игнорировать PickChildrenOnly при скролировании колесом мыши и наведении. Всегда обрабатывать реакцию скролла колесом мыши")]
        [XmlIgnore]
        public bool forceWheel { get; set; }

        [XmlElement("forceWheel")]
        public string _forceWheel
        {
            get => forceWheel.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    forceWheel = result;
            }
        }

        [Category("Track and limit reactions")]
        [Description("Игнорировать двойной клик мышью для виджета и для его детей")]
        [XmlIgnore]
        public bool IgnoreDblClick { get; set; }

        [XmlElement("IgnoreDblClick")]
        public string _IgnoreDblClick
        {
            get => IgnoreDblClick.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    IgnoreDblClick = result;
            }
        }

        [Category("Track and limit reactions")]
        [Description("Является ли виджет прозрачным для ввода. По умолчанию false")]
        [XmlIgnore]
        public bool TransparentInput { get; set; }

        [XmlElement("TransparentInput")]
        public string _TransparentInput
        {
            get => TransparentInput.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    TransparentInput = result;
            }
        }

        [Category("Special")]
        [Description("Запрещать ли пользовательским аддонам операции с виджетом. По умолчанию false")]
        [XmlIgnore]
        public bool isProtected { get; set; }

        [XmlElement("isProtected")]
        public string _isProtected
        {
            get => isProtected.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    isProtected = result;
            }
        }

        [Category("Special")]
        [Description(
            "Задаёт порядок обхода контролов по клавише Tab. По умолчанию 0(не учавствует в обходе). Для участия в обходе значение должно быть больше 0")]
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
        [Description(
            "Уведомление о наведении на виджет вне зависимости от его доступности для кликов. (Использовать только при сильной необходимости - потребляет много ресурсов.)")]
        public string forceReactionOnPointing { get; set; }

        [Category("Mouse reactions")]
        [Description("Уведомление о прокрутке колёсика мыши вверх")]
        public string reactionWheelUp { get; set; }

        [Category("Mouse reactions")]
        [Description("Уведомление о прокрутке колёсика мыши вниз")]
        public string reactionWheelDown { get; set; }

        public virtual ImageSource Bitmap => GetBitmap();

        [Category("Base properties")]
        [Description("Системное название виджета")]
        public string Name { get; set; }

        [Category("Base properties")]
        [Description("Виден ли виджет. По умолчанию true. если виджет не виден, то он недоступен и для реакций")]
        [XmlIgnore]
        public bool Visible { get; set; }

        [Category("Base properties")]
        [Description(
            "Доступен ли виджет и все его дочерние виджеты для реакций. Может влиять на внешний вид (виджет \"засеривается\"). По умолчанию true")]
        [XmlIgnore]
        public bool Enabled { get; set; }

        [Category("Placement")]
        [Description("Описание расположение виджета")]
        public WidgetPlacementXY Placement { get; set; }

        [XmlIgnore] public List<IFile> Widgets => Children?.Select(x => x.File).ToList();

        protected ImageSource GetBitmap()
        {
            return (BackLayer?.File as WidgetLayer)?.Bitmap;
        }
    }
}