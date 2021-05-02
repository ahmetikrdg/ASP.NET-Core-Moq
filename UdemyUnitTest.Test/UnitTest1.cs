using Moq;
using System;
using System.Collections.Generic;
using UdemyUnitTest.APP;
using Xunit;

namespace UdemyUnitTest.Test
{
    public class UnitTest1
    {
        public Calculator calculator { get; set; }
        public Mock<ICalculatorService> mymock { get; set; }//moq tarafýnda taklit edilecek interfaceyi verdik

        public UnitTest1()
        {
            mymock = new Mock<ICalculatorService>();//ýcalculator servicesinin implemenet etmiþ olduðu calculator serviceyi taklit ediyorum
            this.calculator = new Calculator(mymock.Object);//benden bir calculatorservice istiyor neden çünkü benim calculator classýmda ctor ýcalculatorservice istiyor
            //bunuda yukarýda oluþturmuþ olduðum mymock üzerinden vereceðim.
            //calculator nesnem ayaða kalktý ama neyle ayaða kalktý benim sahte olarak taklit ettiðim bir moq nesnem ile ayaða kalktý.

        }
            
        [Fact]
        public void AddTest()
        {
            //Arrange
            int a = 5;
            int b = 20;
            //var calculator = new Calculator();

            //Act
            var total=calculator.add(a, b);

            //Assert
            Assert.Equal<int>(0, total);

        }

        [Theory]
        [InlineData(2,5,7)]
        public void Add_simpleValues_ReturnTotalValue(int a,int b,int expectedtotal)
        {
            mymock.Setup(x => x.add(a, b)).Returns(expectedtotal);
            //mymock üzerinden setup diyorum bir davranýþ belirleyeceðim x le beraber lambdayla içerisinde girelim
            ////add metoduna a ve b yi verdim. eðer ýcalculator service içinde add metodu çaðýrýlýrsa sen return olarak ne döneceksin expectedtotali döneceksin.
            ///taklit etme olayýný gerçekleþtirmiþ oldum
            ///
           
            //var calculator = new Calculator();  best pratice yaptým
            var actualTotal = calculator.add(a, b);
            Assert.Equal(expectedtotal, actualTotal);
            mymock.Verify(x => x.add(a, b), Times.Once);
        }

        [Theory]
        [InlineData(3, 5, 15)]
        public void Multip_SimpleValues_ReturnsMultipValue(int a , int b,int expectedtotal)
        {
            int actualMultip=0;
            mymock.Setup(x => x.multip(It.IsAny<int>(), It.IsAny<int>()))//herhangi bir deðeri kabul edeceðini belirliyorum ve generic olarak int veriyorum. Peki bu gelen deðerli nasýl alýcaz
                .Callback<int, int>((x, y) => actualMultip = x * y);  //iki deðer aldýðý ve int olduðu için belirttim. gelen deðerler iki tane x,y dedim peki ne yapacak bu deðerler. çarpýp bi deðiþkenime atadým

            calculator.multip(a,b);//bu metod çalýþýnca bir üstteki multip çalýþacak ve buradaki deðerli oraya atayacak 
            Assert.Equal(expectedtotal, actualMultip);
            //artýk birden fazla yazabiliriz
            calculator.multip(5, 20);
            Assert.Equal(100, actualMultip);
            
        }

        [Theory]        
        [InlineData(0, 5)]      
        public void Multip_ZeroValues_ReturnsException(int a, int b)
        {//her metotta setup iþlemi gerçekleþir.
            mymock.Setup(x => x.multip(a, b)).Throws(new Exception("a=0 olamaz"));//a0 b5 gelirse bu metot hata dönsün.

            Exception exception= Assert.Throws<Exception>(()=>calculator.multip(a,b));//burdaki calculator.mul.. çalýþýnca geriye exception fýrlatýr
                                                                                      ////eðer thwor exceptionsa hata fýrlatýr.
            //hata mesajýnýda test edelim geriye ne döner exception döner 
            Assert.Equal("a=0 olamaz", exception.Message);//beklediðimz mesajla gelen mesaj aynýmý
        }


    }
}









/// Assert.Contains("Fatih", "Fatih Çakýroðlu");//gerçek ifade içinde fatih geçiyomu
///  var names = new List<string>() { "Fatih", "Emre", "Hasan" };
///Assert.Contains(names, x => x == "Fatih");// x in içinde fatih varmý bunu test et varsa true yoksa false nameyi dönüyor yani

//Assert.True(5 > 2);

//Assert.EndsWith("Bir", "Bir masal");

//Assert.Empty("");

//Assert.InRange(10,2,20);//Hesaplamadan gelen deðerim 10. En düþük deðeri 2 en yüksek 20 verdim.
//yani 10 2 ve 10 arasýndaysa true gelir.

//Assert.Single(new List<string>() { "Ahmet"});

//Assert.IsType<string>("Ahmet");

//Assert.IsAssignableFrom<IEnumerable<string>>(new List<string>());
//Assert.IsAssignableFrom<object>("fatih");//beklediðim deðer mutlaka objeden miras almalý diyorum stringde zaten objeden miras alýr

//string deger = null;
//Assert.Null(deger);//true döner
//Assert.NotNull(deger);//false döner