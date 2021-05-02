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
        public Mock<ICalculatorService> mymock { get; set; }//moq taraf�nda taklit edilecek interfaceyi verdik

        public UnitTest1()
        {
            mymock = new Mock<ICalculatorService>();//�calculator servicesinin implemenet etmi� oldu�u calculator serviceyi taklit ediyorum
            this.calculator = new Calculator(mymock.Object);//benden bir calculatorservice istiyor neden ��nk� benim calculator class�mda ctor �calculatorservice istiyor
            //bunuda yukar�da olu�turmu� oldu�um mymock �zerinden verece�im.
            //calculator nesnem aya�a kalkt� ama neyle aya�a kalkt� benim sahte olarak taklit etti�im bir moq nesnem ile aya�a kalkt�.

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
            //mymock �zerinden setup diyorum bir davran�� belirleyece�im x le beraber lambdayla i�erisinde girelim
            ////add metoduna a ve b yi verdim. e�er �calculator service i�inde add metodu �a��r�l�rsa sen return olarak ne d�neceksin expectedtotali d�neceksin.
            ///taklit etme olay�n� ger�ekle�tirmi� oldum
            ///
           
            //var calculator = new Calculator();  best pratice yapt�m
            var actualTotal = calculator.add(a, b);
            Assert.Equal(expectedtotal, actualTotal);
            mymock.Verify(x => x.add(a, b), Times.Once);
        }

        [Theory]
        [InlineData(3, 5, 15)]
        public void Multip_SimpleValues_ReturnsMultipValue(int a , int b,int expectedtotal)
        {
            int actualMultip=0;
            mymock.Setup(x => x.multip(It.IsAny<int>(), It.IsAny<int>()))//herhangi bir de�eri kabul edece�ini belirliyorum ve generic olarak int veriyorum. Peki bu gelen de�erli nas�l al�caz
                .Callback<int, int>((x, y) => actualMultip = x * y);  //iki de�er ald��� ve int oldu�u i�in belirttim. gelen de�erler iki tane x,y dedim peki ne yapacak bu de�erler. �arp�p bi de�i�kenime atad�m

            calculator.multip(a,b);//bu metod �al���nca bir �stteki multip �al��acak ve buradaki de�erli oraya atayacak 
            Assert.Equal(expectedtotal, actualMultip);
            //art�k birden fazla yazabiliriz
            calculator.multip(5, 20);
            Assert.Equal(100, actualMultip);
            
        }

        [Theory]        
        [InlineData(0, 5)]      
        public void Multip_ZeroValues_ReturnsException(int a, int b)
        {//her metotta setup i�lemi ger�ekle�ir.
            mymock.Setup(x => x.multip(a, b)).Throws(new Exception("a=0 olamaz"));//a0 b5 gelirse bu metot hata d�ns�n.

            Exception exception= Assert.Throws<Exception>(()=>calculator.multip(a,b));//burdaki calculator.mul.. �al���nca geriye exception f�rlat�r
                                                                                      ////e�er thwor exceptionsa hata f�rlat�r.
            //hata mesaj�n�da test edelim geriye ne d�ner exception d�ner 
            Assert.Equal("a=0 olamaz", exception.Message);//bekledi�imz mesajla gelen mesaj ayn�m�
        }


    }
}









/// Assert.Contains("Fatih", "Fatih �ak�ro�lu");//ger�ek ifade i�inde fatih ge�iyomu
///  var names = new List<string>() { "Fatih", "Emre", "Hasan" };
///Assert.Contains(names, x => x == "Fatih");// x in i�inde fatih varm� bunu test et varsa true yoksa false nameyi d�n�yor yani

//Assert.True(5 > 2);

//Assert.EndsWith("Bir", "Bir masal");

//Assert.Empty("");

//Assert.InRange(10,2,20);//Hesaplamadan gelen de�erim 10. En d���k de�eri 2 en y�ksek 20 verdim.
//yani 10 2 ve 10 aras�ndaysa true gelir.

//Assert.Single(new List<string>() { "Ahmet"});

//Assert.IsType<string>("Ahmet");

//Assert.IsAssignableFrom<IEnumerable<string>>(new List<string>());
//Assert.IsAssignableFrom<object>("fatih");//bekledi�im de�er mutlaka objeden miras almal� diyorum stringde zaten objeden miras al�r

//string deger = null;
//Assert.Null(deger);//true d�ner
//Assert.NotNull(deger);//false d�ner