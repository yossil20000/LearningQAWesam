using LearningQA.Client.Model;
using LearningQA.Shared.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningQA.Client.ViewModel
{
	public interface IExamViewModel
	{
		ExamViewModelPersist ExamViewModelPersist { get; set; }
		Task RetriveTestItemInfos(int testItemId);
		
		Task<List<ExamInfoModel>> RetriveExamInfoModels();
		Task OnLoadCommand();
		Task OnTestId(int testId);
		//TestItem<QUestionSql, int> TestItem { get; set; }
	
		


	}
	public class ExamViewModel : IExamViewModel
	{
		public ITestItemModel testItemModel;
		public ExamViewModelPersist ExamViewModelPersist { get; set; }
		public ExamViewModel(ITestItemModel testItemModel, ExamViewModelPersist examViewModelPersist)
		{
			this.testItemModel = testItemModel;
			ExamViewModelPersist = examViewModelPersist;

		}
		public async Task<List<ExamInfoModel>> RetriveExamInfoModels()
		{
			var ressult = await testItemModel.RetriveExamInfoModels();
			return ressult;
		}
		public async Task RetriveTestItemInfos(int testItemId)
		{
			if (ExamViewModelPersist.Initialize)
				return;
			await testItemModel.RetriveTestItemInfos();
			ExamViewModelPersist.TestItemInfos = testItemModel.TestItemInfos.ToList();

			//TestItemViewModelPersist.SelectedQuestion = TestItemViewModelPersist.TestItem.Questions.ElementAt(0);
			//TestItemViewModelPersist.CurrentQuestion = 1;
			//TestItemViewModelPersist.EnablePreviouse = false;
			//if (TestItemViewModelPersist.TestItem.Questions.Count > 1)
			//	TestItemViewModelPersist.EnableNext = true;
		}
		public async Task OnTestId(int testId)
		{
			try
			{
				var result = await testItemModel.LoadTest(testId);
				if (result != null)
				{
					var testItemInfo = await testItemModel.RetriveTestItemInfo(result.TestItemId);
					if (testItemInfo != null)
					{
						ExamViewModelPersist.SelectedCategory = testItemInfo.Category;
						ExamViewModelPersist.SelectedSubjecte = testItemInfo.Subject;
						ExamViewModelPersist.SelectedChapter = testItemInfo.Chapter;
						//TestItemViewModelPersist.CurrentTest = result;
						//TestItemViewModelPersist.CurrentTest.Answers = result.Answers.OrderBy(x => int.Parse(x.QUestionSql.QuestionNumber)).ToList();
						//for (int i = 0; i < TestItemViewModelPersist.CurrentTest.Answers.Count(); i++)
						//{
						//	var item = TestItemViewModelPersist.CurrentTest.Answers.ElementAt(i).SelectedAnswer;
						//	if (item == null)
						//	{
						//		item = new List<AnswareOption<int>>();
						//		TestItemViewModelPersist.CurrentTest.Answers.ElementAt(i).SelectedAnswer = item;
						//	}
						//}
						//TestItemViewModelPersist.CurrentQuestion = 1;
						//TestItemViewModelPersist.CurrentTest.Duration = result.Duration;
						//TestItemViewModelPersist.SelectedQuestion = TestItemViewModelPersist.CurrentTest.Answers.ElementAt(0).QUestionSql;
						//TestItemViewModelPersist.Changed();
					}

				}


			}
			catch (Exception ex)
			{


			}
		}

		

		public async Task OnLoadCommand()
		{
			try
			{
				TestItemInfo testItemInfo = new TestItemInfo()
				{
					Category = ExamViewModelPersist.SelectedCategory,
					Subject = ExamViewModelPersist.SelectedSubjecte,
					Chapter = ExamViewModelPersist.SelectedChapter
				};
				var result = await testItemModel.RetriveExamInfoModels();
				if(result != null)
				{
					ExamViewModelPersist.ExamInfoModels = result;
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine($"ExamVieModel.OnLoadCommand {ex.Message}");
			}
			 await Task.CompletedTask;
		}
	}
}
