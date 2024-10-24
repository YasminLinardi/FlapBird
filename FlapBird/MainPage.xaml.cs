namespace FlapBird;

public partial class MainPage : ContentPage

{
	const int gravidade = 3;
	const int tempoEntreFrames = 25;
	const int maxTempoPulando = 3;
	const int forcaPulo = 15;
	const int aberturaMinima = 10;
	bool morto = true;
	bool estaPulando = false;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 20;
	int tempoPulando = 0;
	int score = 0;



	public MainPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		larguraJanela = width;
		alturaJanela = height;
		if (height > 0)
		{
			CanoC.HeightRequest = height - Chao.HeightRequest;
			CanoB.HeightRequest = height - Chao.HeightRequest;
		}
	}

	void Inicializar()
	{
		morto = false;
		CanoC.TranslationX = -larguraJanela;
		CanoC.TranslationY = -larguraJanela;
		Slime.TranslationX = 0;
		Slime.TranslationY = 0;
		score = 0;
		GerenciaCanos();
	}

	async Task Desenhar()
	{
		while (!morto)
		{
			if (estaPulando)
				AplicaPulo();
			else
				AplicarGravidade();
			GerenciaCanos();
			if (VerificaColisao())
			{
				morto = true;
				labelGameOver.Text = "Você passou por\n" + score + " canos";
				frameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(tempoEntreFrames);
		}
	}

	void AplicaPulo()
	{
		Slime.TranslationY -= forcaPulo;
		tempoPulando++;
		if (tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
	}

	bool VerificaColisao()
	{
		    return (!morto && 
					(VerificaColisaoChao() ||
			 		VerificaColisaoteto() ||
			  		VerificaColisaoCano()));

	}

	  bool VerificaColisaoCano()
  	{
    if (VerificaColisaoCanoB() || VerificaColisaoCanoC())
      return true;
    else
      return false;
  	}

	bool VerificaColisaoteto()
	{
    var alturaDoTeto = alturaJanela / 2;
    if (Slime.TranslationY <= -alturaDoTeto)
      return true;
    else
      return false;
  }

	void AplicarGravidade()
	{
		Slime.TranslationY += gravidade;
	}

	void GerenciaCanos()
	{
		CanoC.TranslationX -= velocidade;
		CanoB.TranslationX -= velocidade;
		if (CanoB.TranslationX < -larguraJanela)
		{
			CanoB.TranslationX = 0;
			CanoB.TranslationX = 0;
			var alturaMax = -(CanoB.HeightRequest * 0.1);
			var alturaMin = -(CanoB.HeightRequest * 0.8);
			CanoC.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			CanoB.TranslationY = CanoC.HeightRequest + CanoC.TranslationY + aberturaMinima;
			score++;
			labelScore.Text = "Score: " + score.ToString("D5");
			if (score % 4 == 0)
			velocidade++;
		}
	}

	void OnGameOverClicked(object s, TappedEventArgs a)
	{
		frameGameOver.IsVisible = false;
		 Inicializar();
    	 morto = false;
    	 Desenhar().RunSynchronously();

	}

	void OnGridClicked(object s, TappedEventArgs a)
	{
		estaPulando = true;
	}

	bool VerificaColisaoCanoC()
	{
		var posHSlime = (larguraJanela / 2) - (Slime.WidthRequest / 2);
		var posVSlime = (alturaJanela / 2) - (Slime.HeightRequest / 2) + Slime.TranslationY;
		if (posHSlime >= Math.Abs(CanoC.TranslationX) - CanoC.WidthRequest &&
			posHSlime <= Math.Abs(CanoC.TranslationX) - CanoC.WidthRequest &&
			posVSlime <= CanoC.HeightRequest + CanoC.TranslationY)
		{
			return true;
		}
			return false;
	}

	bool VerificaColisaoCanoB()
	{
		var posHSlime = (larguraJanela / 2) - (Slime.WidthRequest / 2);
		var posVSlime = (alturaJanela / 2) - (Slime.HeightRequest / 2) + Slime.TranslationY;
		if (posHSlime >= Math.Abs(CanoB.TranslationX) - CanoB.WidthRequest &&
			posHSlime <= Math.Abs(CanoB.TranslationX) - CanoB.WidthRequest &&
			posVSlime >= CanoB.WidthRequest + CanoB.TranslationY)
		{
			return true;
		}
			return false;


	}

		bool VerificaColisaoChao()
	{
		var maxY = alturaJanela / 2;
		if (Slime.TranslationY >= alturaDoTeto - Chao.Height)
		{
			return true;
		}
			return false;
	}


}