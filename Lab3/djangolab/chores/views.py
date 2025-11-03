# Create your views here.

from django.shortcuts import render
from .models import ChoreLaborer, ChoreWorkforce

def index(request):
    return render(request, 'chores/index.html')

def laborers(request):
    # Manually create ChoreLaborer objects
    workforce = ChoreWorkforce()
    
    # Add some sample laborers
    laborer1 = ChoreLaborer("Alice", 25, 7)
    laborer2 = ChoreLaborer("Bob", 30, 4)
    laborer3 = ChoreLaborer("Charlie", 22, 9)
    
    workforce.add_laborer(laborer1)
    workforce.add_laborer(laborer2)
    workforce.add_laborer(laborer3)
    
    context = {
        'workforce': workforce
    }
    return render(request, 'chores/laborers.html', context)
