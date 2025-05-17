using LinkBotLogic.Exceptions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkBotLogic.Pages
{
    public class MessagingPage : BasePage
    {
        private readonly By _overlayToggle = By.Id("msg-overlay-list-bubble-header__button");
        private readonly By _messageInput = By.CssSelector("div.msg-form__contenteditable");
        private readonly By _contactName = By.CssSelector("li.msg-s-message-list__event.clearfix[class*='msg-s-message-list__last-msg-ember']");
        private readonly By _sendBtn = By.CssSelector(".msg-form__send-button.artdeco-button.artdeco-button--1");
        private readonly By _msgContainer = By.CssSelector("div.msg-form__contenteditable[contenteditable='true']");
        private readonly By _contactBox = By.CssSelector(".scrollable.msg-overlay-list-bubble__content.msg-overlay-list-bubble__content--scrollable");
        private readonly By _contacts = By.CssSelector(".entry-point");
        private readonly By _nameSpan = By.XPath(@"
                                .//div[contains(@class, 'msg-overlay-list-bubble__convo-card-content')]
                                //div[contains(@class, 'msg-conversation-card__row')]
                                //h3//div//span
                                ");

        public MessagingPage(IWebDriver driver, string targetText)
            : base(driver, TimeSpan.FromSeconds(20))
        { }

        public void Open()
        {
            Click(_overlayToggle);
        }
        public async Task SendMessage(string targetText)
        {
            InitSendMsg(targetText);
            await SendMessageLogic(targetText);
        }

        private async Task SendMessageLogic(string targetText)
        {

            IWebElement msgContainer = Find(_msgContainer);

            msgContainer.Click(); // Ensure the element is focused

            //Validate if last message is from destinatary
            bool valid = this.ValidateMessage(targetText);

            if (valid)
            {
                IWebElement btnSend = Find(_sendBtn);

                if (msgContainer.Text.Contains("👍"))
                {
                    btnSend.Click();
                }
                else
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                    string script = "document.execCommand('insertText', false, arguments[1]);";
                    js.ExecuteScript(script, msgContainer, "👍");

                    var timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
                    if (await timer.WaitForNextTickAsync())
                    {
                        btnSend.Click();
                    }
                }
            }
        }

        private void InitSendMsg(string targetText)
        {
            //Contact box
            IWebElement contactsDiv = Find(_contactBox);

            //Find all contacts
            ReadOnlyCollection<IWebElement> contacts = FindListNested(_contacts, contactsDiv);

            //Check if contact match desired value and select it
            IWebElement? targetContact = contacts.FirstOrDefault(c =>
            {
                IWebElement nameSpan = FindNested(c, _nameSpan);
                return nameSpan.Text.Trim().Equals(targetText, StringComparison.OrdinalIgnoreCase);
            });

            targetContact.Click();
        }

        private bool ValidateMessage(string targetText)
        {
            string? contactNameText = Find(_contactName).Text;
            if (contactNameText.Contains(targetText))
            {
                return true;
            }

            return false;
        }
    }
}